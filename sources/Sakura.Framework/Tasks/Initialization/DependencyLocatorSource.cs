namespace Sakura.Framework.Tasks.Initialization
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Sakura.Framework.Dependencies;
    using Sakura.Framework.ExtensionMethods;
    using Sakura.Framework.Internal;

    public class DependencyLocatorSource : IInitializationTaskSource
    {
        private readonly IDependencyLocator locator;

        public DependencyLocatorSource(IDependencyLocator locator)
        {
            this.locator = locator;
        }

        public IEnumerable<IInitializationTask> GetTasks()
        {
            var taskTypes = this.locator.GetDependencies().Where(type => type.HasInterface(typeof(IInitializationTask)));

            foreach (var taskType in taskTypes)
            {
                this.VerifyTaskType(taskType);

                var factory = new ObjectCreateMethod(taskType);

                yield return factory.CreateInstance<IInitializationTask>();
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