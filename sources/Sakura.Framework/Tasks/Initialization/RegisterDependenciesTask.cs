namespace Sakura.Framework.Tasks.Initialization
{
    using System;
    using System.Linq;

    using Sakura.Framework.Dependencies;

    [NotDiscoverable]
    public class RegisterDependenciesTask : IInitializationTask, ITransientDependency
    {
        private readonly IDependencyLocator locator;

        public RegisterDependenciesTask(IDependencyLocator locator)
        {
            this.locator = locator;
        }

        public void Execute(InitializationTaskContext context)
        {
            var dependencyTypes = this.locator.GetDependencies(context.Policies);

            foreach (var dependencyType in dependencyTypes)
            {
                Register(dependencyType, context);
            }
        }

        private static void Register(Type dependencyType, InitializationTaskContext context)
        {
            var builder = context.Builder;
            var policies = context.Policies;

            foreach (var policy in policies.Where(p => p.IsMatch(dependencyType)))
            {
                policy.Apply(dependencyType, builder);
            }
        }
    }
}