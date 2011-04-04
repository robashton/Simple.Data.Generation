using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NUnit.Framework;
using Simple.Data.Generation.Tests.Model;

namespace Simple.Data.Generation.Tests
{
    [TestFixture]
    public class Integration
    {
        [Test]
        public void Simple_Query_Against_Table_Results_In_String_Property_Being_Discovered()
        {
            var scanner = new ModelScanner();
            var model = scanner.CreateModelFromAssembly<Users>();
            var table = model.GetTable("Users");
            var property = table.GetColumn("Username");
            Assert.That(property.Type, Is.EqualTo(typeof(string)));
        }
    }

    [TestFixture]
    public class MethodInspectionTests
    {
        [Test]
        public void Can_Extract_Dynamic_Method_Invocations_From_A_Method()
        {
            var inspector = new MethodInspectorBuilder()
                .ForClass<ClassWithSomeDynamicUsages>()
                .WithMethod("MethodWithSingleDynamicCall")
                .Get();

            var methodInvocations = inspector.ExtractDynamicMethodCallInstructions();
            Assert.That(methodInvocations, Has.Length.EqualTo(1));
        }

        [Test]
        public void Can_Extract_Dynamic_Cached_Fields_From_A_Method()
        {
            var inspector = new MethodInspectorBuilder()
                .ForClass<ClassWithSomeDynamicUsages>()
                .WithMethod("MethodWithSingleDynamicCall")
                .Get();

            var cachedFields = inspector.ExtractReflectedCachedFields();
            Assert.That(cachedFields, Has.Length.EqualTo(1));
        }
    }

    public class MethodInspectorBuilder
    {
        private Type type;
        private string method;

        public MethodInspectorBuilder ForClass<T>()
        {
            type = typeof (T);
            return this;
        }

        public MethodInspectorBuilder WithMethod(string methodName)
        {
            this.method = methodName;
            return this;
        }

        public MethodInspector Get()
        {
            var assembly = type.Assembly;
            var assemblyDefinition = Mono.Cecil.AssemblyDefinition.ReadAssembly(assembly.Location);
            var typeInformation = assemblyDefinition.MainModule.Types.Single(x => x.FullName == type.FullName);
            var methodInformation = typeInformation.Methods.Single(x => x.Name == this.method);
            return new MethodInspector(methodInformation);
        }
    }

    public class ModelScanner
    {
        public DataModel CreateModelFromAssembly<T>()
        { 
            var model = new DataModel();
            var assembly = typeof(T).Assembly;

            var assemblyDefinition = Mono.Cecil.AssemblyDefinition.ReadAssembly(assembly.Location);

            assemblyDefinition.MainModule.Types
                .Select(type => new TypeInspector(type)).ToList()
                .ForEach(inspector => inspector.PopulateDataModel(model));

            return model;
        }
    }

    public class TypeInspector
    {
        private readonly TypeDefinition definition;

        public TypeInspector(TypeDefinition definition)
        {
            this.definition = definition;
        }  

        public void PopulateDataModel(DataModel model)
        {
            definition.Methods
                .Select(method =>new MethodInspector(method)).ToList()
                .ForEach(inspector => inspector.PopulateDataModel(model));
        }
    }

    public class MethodInspector
    {
        private readonly MethodDefinition method;

        public MethodInspector(MethodDefinition method)
        {
            this.method = method;
        }

        public void PopulateDataModel(DataModel model)
        {
            Instruction[] dynamicMethodCalls = ExtractDynamicMethodCallInstructions();
            if (dynamicMethodCalls.Length == 0) { return; }

            FieldDefinition[] cachedReflectedFields = ExtractReflectedCachedFields();

            return;
        }

        public FieldDefinition[] ExtractReflectedCachedFields()
        {
           return method.Body.Instructions
                .Where(x => x.OpCode.Code == Code.Ldsfld)
                .Select(x=> (FieldReference)x.Operand)
                .Distinct()
                .Where(x=> x.IsCachedReflectionField())
                .Select(x=>x.Resolve())
                .ToArray();
        }

        public Instruction[] ExtractDynamicMethodCallInstructions()
        {
            return method.Body.Instructions
                .Where(x => x.OpCode.Code == Mono.Cecil.Cil.Code.Callvirt)
                .Where(x => x.Operand is MemberReference && ((MemberReference)(x.Operand)).Name == "Invoke")
                .ToArray();
        }
    }

    public static class ReflectionExtensions
    {
        public static bool IsCachedReflectionField(this FieldReference definition)
        {
            var baseType = definition.FieldType.Resolve().BaseType;
            return baseType.FullName == typeof (System.Runtime.CompilerServices.CallSite).FullName;
            return false;
        }
    }

    public class DataModel
    {
        private readonly ConcurrentDictionary<string, DataModelTable> tables =
            new ConcurrentDictionary<string, DataModelTable>(StringComparer.InvariantCultureIgnoreCase);


        public DataModelTable GetTable(string tableName)
        {
            DataModelTable table = null;
            if (tables.TryGetValue(tableName, out table)) return table;
            return null;
        }
    }

    public class DataModelTable
    {
        private readonly ConcurrentDictionary<string, DataModelTableColumn> columns =
    new ConcurrentDictionary<string, DataModelTableColumn>(StringComparer.InvariantCultureIgnoreCase);

        public DataModelTableColumn GetColumn(string columnName)
        {
            DataModelTableColumn column = null;
            if (columns.TryGetValue(columnName, out column)) return column;
            return column;
        }
    }

    public class DataModelTableColumn
    {
        public Type Type { get; private set; }
    }
}
