// Type: Mono.Cecil.FieldReference
// Assembly: Mono.Cecil, Version=0.9.4.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756
// Assembly location: C:\Stuff\Research\Simple.Data.Generation\packages\Mono.Cecil.0.9.4.0\lib\20\Mono.Cecil.dll

namespace Mono.Cecil
{
    public class FieldReference : MemberReference
    {
        public FieldReference(string name, TypeReference fieldType);
        public FieldReference(string name, TypeReference fieldType, TypeReference declaringType);
        public TypeReference FieldType { get; set; }
        public override string FullName { get; }
        public virtual FieldDefinition Resolve();
    }
}
