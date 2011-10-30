namespace Fugu.Framework.Dependencies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Fugu.Framework.ExtensionMethods;

    public class DependencyLocator : IDependencyLocator
    {
        private readonly IEnumerable<Assembly> assemblies;

        public DependencyLocator(IEnumerable<Assembly> assemblies)
        {
            this.assemblies = assemblies;
        }

        public IEnumerable<Type> GetDependencies()
        {
            foreach (var assembly in assemblies)
            {
                var dependencyTypes = assembly.GetExportedTypes().Where(type => TypeExtensions.HasInterface(type, typeof(IDependency)) && type.IsClass && !type.IsAbstract);

                foreach (var dependencyType in dependencyTypes)
                {
                    yield return dependencyType;
                }
            }
        }
    }
}