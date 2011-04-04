using System.Linq;

namespace Simple.Data.Generation
{
    public class ModelScanner
    {
        public DataModel CreateModelFromAssembly<T>()
        { 
            var model = new DataModel();
            var assembly = typeof(T).Assembly;

            var assemblyDefinition = Mono.Cecil.AssemblyDefinition.ReadAssembly(assembly.Location);

                 assemblyDefinition.MainModule.Types
                .Select(type => new TypeInspector(type))
                .ToList()
                .ForEach(inspector => inspector.PopulateDataModel(model));

            return model;
        }
    }
}