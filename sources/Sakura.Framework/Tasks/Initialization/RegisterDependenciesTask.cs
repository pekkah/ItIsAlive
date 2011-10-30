namespace Sakura.Framework.Tasks.Initialization
{
    using System;
    using System.Linq;

    using Autofac;

    using Sakura.Framework.Dependencies;
    using Sakura.Framework.ExtensionMethods;

    [NotDiscoverable]
    public class RegisterDependenciesTask : IInitializationTask
    {
        private readonly IDependencyLocator locator;

        public RegisterDependenciesTask(IDependencyLocator locator)
        {
            this.locator = locator;
        }

        public void Execute(InitializationTaskContext context)
        {
            var dependencyTypes = this.locator.GetDependencies();

            foreach (var dependencyType in dependencyTypes)
            {
                Register(dependencyType, context);
            }
        }

        private static void Register(Type dependencyType, InitializationTaskContext context)
        {
            var builder = context.Builder;

            var registration = builder.RegisterType(dependencyType).AsSelf();

            var dependencyInterfaces = dependencyType.GetInterfaces().Where(
                itf => itf.HasInterface(typeof(IDependency)));

            foreach (var dependencyInterface in dependencyInterfaces)
            {
                registration = registration.As(dependencyInterface);

                if (typeof(ISingleInstanceDependency).IsAssignableFrom(dependencyInterface))
                {
                    registration = registration.SingleInstance();
                }
                else if (typeof(ITransientDependency).IsAssignableFrom(dependencyInterface))
                {
                    registration = registration.InstancePerDependency();
                }
            }
        }
    }
}