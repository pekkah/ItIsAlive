namespace Sakura.Framework.Dependencies
{
    using System;
    using System.Linq;

    using Sakura.Bootstrapping.Tasks.Types;
    using Sakura.Framework.Dependencies.DefaultTypes;
    using Sakura.Framework.Dependencies.Discovery;

    [NotDiscoverable]
    public class RegisterDependencies : IInitializationTask, ITransientDependency
    {
        private readonly IDependencyLocator locator;

        public RegisterDependencies(IDependencyLocator locator)
        {
            this.locator = locator;
        }

        public void Execute(InitializationTaskContext context)
        {
            var dependencyTypes = this.locator.GetDependencies(context.Conventions);

            foreach (var dependencyType in dependencyTypes)
            {
                Register(dependencyType, context);
            }
        }

        private static void Register(Type dependencyType, InitializationTaskContext context)
        {
            var builder = context.Builder;
            var conventions = context.Conventions;

            foreach (var policy in conventions.Where(p => p.IsMatch(dependencyType)))
            {
                policy.Apply(dependencyType, builder);
            }
        }
    }
}