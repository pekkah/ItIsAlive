namespace Sakura.Bootstrapping
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class ConfigureDependencies : IConfigureDependencies
    {
        private readonly List<Assembly> assemblyList;

        private readonly List<Type> typeList;

        public ConfigureDependencies()
        {
            this.assemblyList = new List<Assembly>();
            this.typeList = new List<Type>();
        }

        public void Assemblies(params Assembly[] assemblies)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException("assemblies");
            }

            this.assemblyList.AddRange(assemblies);
        }

        public void Assembly(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            this.assemblyList.Add(assembly);
        }

        public void AssemblyOf<T>()
        {
            var assembly = typeof(T).Assembly;

            this.assemblyList.Add(assembly);
        }

        public IEnumerable<Assembly> SourceAssemblies
        {
            get
            {
                return this.assemblyList;
            }
        }

        public IEnumerable<Type> SourceTypes
        {
            get
            {
                return this.typeList;
            }
        }

        public void Types(params Type[] dependencyTypes)
        {
            this.typeList.AddRange(dependencyTypes);
        }
    }
}