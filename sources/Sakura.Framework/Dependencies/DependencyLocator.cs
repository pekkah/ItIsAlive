namespace Sakura.Framework.Dependencies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Sakura.Framework.ExtensionMethods;

    public class DependencyLocator : IDependencyLocator
    {
        private readonly IEnumerable<Assembly> assemblies;

        public DependencyLocator(IEnumerable<Assembly> assemblies)
        {
            this.assemblies = assemblies;
        }

        public IEnumerable<Type> GetDependencies()
        {
            foreach (var assembly in this.assemblies)
            {
                var dependencyTypes =
                    assembly.GetExportedTypes().Where(
                        type =>
                        type.HasInterface(typeof(IDependency)) && type.IsClass && !type.IsAbstract);

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
}