namespace ItIsAlive
{
    using System;
    using System.Reflection;

    public interface IConfigureDependencies
    {
        void Assemblies(params Assembly[] assemblies);

        void Assembly(Assembly assembly);

        void AssemblyOf<T>();

        void Types(params Type[] dependencyTypes);
    }
}