namespace Sakura.Bootstrapping.Setup
{
    using System;
    using System.Reflection;

    public interface ISetupDependencies
    {
        void Assemblies(params Assembly[] assemblies);

        void Assembly(Assembly assembly);

        void AssemblyOf<T>();

        void Types(params Type[] dependencyTypes);
    }
}