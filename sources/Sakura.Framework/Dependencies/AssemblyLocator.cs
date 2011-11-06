namespace Sakura.Framework.Dependencies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Sakura.Framework.ExtensionMethods;

    public class AssemblyLocator : IDependencyLocator
    {
        private readonly IEnumerable<Assembly> assemblies;

        public AssemblyLocator(IEnumerable<Assembly> assemblies)
        {
            this.assemblies = assemblies;
        }

        public IEnumerable<Type> GetDependencies()
        {
            foreach (var assembly in this.assemblies)
            {
                foreach (var type1 in FilterDependencyTypes(assembly.GetExportedTypes())) yield return type1;
            }
        }

        internal static IEnumerable<Type> FilterDependencyTypes(IEnumerable<Type> types)
        {
            if (types == null)
            {
                throw new ArgumentNullException("types");
            }

            var dependencyTypes = types.Where(
                    type => type.HasInterface(typeof(IDependency)) && type.IsClass && !type.IsAbstract);

            foreach (var dependencyType in dependencyTypes)
            {
                // skip non discoverable dependencies
                if (Attribute.IsDefined(dependencyType, typeof(NotDiscoverable)))
                {
                    continue;
                }

                yield return dependencyType;
            }
        }
    }
}