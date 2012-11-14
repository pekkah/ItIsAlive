namespace ItIsAlive
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class ConfigureDependencies : IConfigureDependencies
    {
        private readonly List<Assembly> _assemblyList;

        private readonly List<Type> _typeList;

        public ConfigureDependencies()
        {
            _assemblyList = new List<Assembly>();
            _typeList = new List<Type>();
        }

        public IEnumerable<Assembly> SourceAssemblies
        {
            get { return _assemblyList; }
        }

        public IEnumerable<Type> SourceTypes
        {
            get { return _typeList; }
        }

        public void Assemblies(params Assembly[] assemblies)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException("assemblies");
            }

            foreach (Assembly assembly in assemblies)
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

            if (_assemblyList.Contains(assembly))
            {
                return;
            }

            _assemblyList.Add(assembly);
        }

        public void AssemblyOf<T>()
        {
            Assembly assembly = typeof (T).Assembly;

            Assembly(assembly);
        }

        public void Types(params Type[] dependencyTypes)
        {
            _typeList.AddRange(dependencyTypes);
        }
    }
}