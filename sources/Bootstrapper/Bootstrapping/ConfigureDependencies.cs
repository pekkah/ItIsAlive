namespace Bootstrapper.Bootstrapping
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
            assemblyList = new List<Assembly>();
            typeList = new List<Type>();
        }

        public IEnumerable<Assembly> SourceAssemblies
        {
            get
            {
                return assemblyList;
            }
        }

        public IEnumerable<Type> SourceTypes
        {
            get
            {
                return typeList;
            }
        }

        public void Assemblies(params Assembly[] assemblies)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException("assemblies");
            }

            foreach (var assembly in assemblies)
            {
                Assembly(assembly);
            }
        }

        public void Assembly(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            if (assemblyList.Contains(assembly))
            {
                return;
            }

            assemblyList.Add(assembly);
        }

        public void AssemblyOf<T>()
        {
            var assembly = typeof(T).Assembly;

            Assembly(assembly);
        }

        public void Types(params Type[] dependencyTypes)
        {
            typeList.AddRange(dependencyTypes);
        }
    }
}