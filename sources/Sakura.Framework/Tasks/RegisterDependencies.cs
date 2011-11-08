namespace Sakura.Framework.Tasks
{
    using System;
    using System.Linq;

    using Sakura.Framework.Dependencies.DefaultTypes;
    using Sakura.Framework.Dependencies.Discovery;
    using Sakura.Framework.Tasks.Types;

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