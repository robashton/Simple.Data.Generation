using System;
using System.Linq;

namespace Simple.Data.Generation.Tests
{
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
}