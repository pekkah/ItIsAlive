namespace Sakura.Bootstrapping.Tasks.Discovery
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Sakura.Bootstrapping.Tasks.Types;
    using Sakura.ExtensionMethods;
    using Sakura.Framework.Dependencies.Conventions;
    using Sakura.Framework.Dependencies.Discovery;

    public class DependencyLocatorProvider : IInitializationTaskProvider
    {
        private readonly IDependencyLocator locator;

        private readonly IEnumerable<IRegistrationConvention> policies;

        private List<IInitializationTask> tasks;

        private bool tasksLoaded;

        public DependencyLocatorProvider(IDependencyLocator locator, IEnumerable<IRegistrationConvention> policies)
        {
            this.locator = locator;
            this.policies = policies;
        }

        public IEnumerable<IInitializationTask> Tasks
        {
            get
            {
               if (!this.tasksLoaded)
               {
                   this.tasks = this.LoadTasks().ToList();
                   this.tasksLoaded = true;
               }

                return this.tasks;
            }
        }

        private IEnumerable<IInitializationTask> LoadTasks()
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