namespace Sakura.Framework.Tasks.Discovery
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Sakura.Framework.Dependencies.Discovery;
    using Sakura.Framework.Dependencies.Policies;
    using Sakura.Framework.ExtensionMethods;
    using Sakura.Framework.Tasks.Types;

    public class DependencyLocatorSource : IInitializationTaskSource
    {
        private readonly IDependencyLocator locator;

        private readonly IEnumerable<IRegistrationPolicy> policies;

        public DependencyLocatorSource(IDependencyLocator locator, IEnumerable<IRegistrationPolicy> policies)
        {
            this.locator = locator;
            this.policies = policies;
        }

        public IEnumerable<IInitializationTask> GetTasks()
        {
            var taskTypes =
                this.locator.GetDependencies(this.policies).Where(
                    type => type.HasInterface(typeof(IInitializationTask)));

            foreach (var taskType in taskTypes)
            {
                this.VerifyTaskType(taskType);

                yield return Activator.CreateInstance(taskType) as IInitializationTask;
            }
        }

        [Conditional("DEBUG")]
        private void VerifyTaskType(Type taskType)
        {
            var constructorInfo = taskType.GetConstructor(Type.EmptyTypes);

            if (constructorInfo == null)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "The type '{0}' implementing IInitializationTask does not have a empty constructor.", 
                        taskType.FullName));
            }
        }
    }
}