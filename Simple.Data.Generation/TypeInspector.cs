using System.Linq;
using Mono.Cecil;

namespace Simple.Data.Generation
{
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
}