namespace Fugu.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Autofac;

    using Fugu.Framework.Tasks;

    public interface ISetupBootstrapper
    {
        ISetupBootstrapper DependenciesFrom(Assembly assembly);

        ISetupBootstrapper DependenciesFrom(IEnumerable<Assembly> assemblies);

        ISetupBootstrapper DependenciesFrom(Type assemblyOfType);

        ISetupBootstrapper ExposeContainer(Action<IContainer> exposeTo);

        Bootstrapper Start();

        ISetupBootstrapper Task(IInitializationTask task);
    }
}