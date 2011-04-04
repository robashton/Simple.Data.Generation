using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Simple.Data.Generation
{
    public static class ReflectionExtensions
    {
        public static bool IsCachedReflectionField(this FieldReference definition)
        {
            var baseType = definition.FieldType.Resolve().BaseType;
            return baseType.FullName == typeof (System.Runtime.CompilerServices.CallSite).FullName;
            return false;
        }

        public static string ParseToFieldReferenceCreation(this Instruction fieldReference)
        {
            Instruction current = fieldReference;
            while(current != null && current.OpCode.Code != Code.Ldstr)
            {
                current = current.Next;
            }
            return (string)current.Operand;
        }
    }
}