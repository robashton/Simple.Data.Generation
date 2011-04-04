using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Simple.Data.Generation
{
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

            Instruction[] cachedReflectedFields = ExtractInitialReflectedCachedFieldReferenceInstructions();
            if (cachedReflectedFields.Length == 0) { return; }

            var references = cachedReflectedFields
                .Select(field => field.ParseToFieldReferenceCreation())
                .Reverse<string>()
                .ToList<string>();

            if (references.Count != 2) { return; }

            string tableName = references[0];
            string columnName = references[1].Replace("FindBy", "");
            Type columnType = null;

            var actualCall = dynamicMethodCalls.Last();

            var current = actualCall.Previous;
            if(current.OpCode.Code == Code.Ldstr)
            {
                columnType = typeof (string);
            }

            model.Table(tableName)
                .Column(columnName)
                .SetType(columnType);
        }

        public Instruction[] ExtractInitialReflectedCachedFieldReferenceInstructions()
        {
            return method.Body.Instructions
                .Where(x => x.OpCode.Code == Code.Ldsfld && x.Next.OpCode.Code == Code.Brtrue_S)
                .Where(x=> ((FieldReference)x.Operand).IsCachedReflectionField())
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
}