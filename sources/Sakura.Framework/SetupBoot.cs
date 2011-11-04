namespace Sakura.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Autofac;

    using Sakura.Framework.Dependencies;
    using Sakura.Framework.Tasks;
    using Sakura.Framework.Tasks.Initialization;

    public class SetupBoot : ISetupBootstrapper
    {
        private readonly List<Assembly> assemblyList;

        private readonly List<IInitializationTask> taskList;

        private readonly List<IInitializationTask> tryAdddingTasksList; 

        private Action<IContainer> exposeContainer;

        public SetupBoot()
        {
            this.assemblyList = new List<Assembly>();
            this.taskList = new List<IInitializationTask>();
            this.tryAdddingTasksList = new List<IInitializationTask>();
        }

        public ISetupBootstrapper DependenciesFrom(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            this.assemblyList.Add(assembly);

            return this;
        }

        public ISetupBootstrapper DependenciesFrom(IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException("assemblies");
            }

            this.assemblyList.AddRange(assemblies);

            return this;
        }

        public ISetupBootstrapper DependenciesFrom(Type assemblyOfType)
        {
            var assembly = assemblyOfType.Assembly;

            return this.DependenciesFrom(assembly);
        }

        public ISetupBootstrapper Dependencies(params Type[] dependencyTypes)
        {
            var locator = new DependencyListLocator(dependencyTypes);

            return this.Task(new RegisterDependenciesTask(locator));
        }

        public ISetupBootstrapper TryTask(IInitializationTask http)
        {
            this.tryAdddingTasksList.Add(http);

            return this;
        }

        public ISetupBootstrapper ExposeContainer(Action<IContainer> exposeTo)
        {
            if (exposeTo == null)
            {
                throw new ArgumentNullException("exposeTo");
            }

            this.exposeContainer = exposeTo;

            return this;
        }

        public Bootstrapper Start()
        {
            var locator = new DependencyLocator(this.assemblyList);

            var bootstrapper = new Bootstrapper();

            // load dependencies from locator
            bootstrapper.Tasks.AddTask(new RegisterDependenciesTask(locator));

            // load initialization tasks from locator
            bootstrapper.Tasks.AddTaskSource(new DependencyLocatorSource(locator));

            // add tasks
            this.taskList.ForEach(bootstrapper.Tasks.AddTask);

            // try adding tasks. Will only add tasks once.
            this.tryAdddingTasksList.ForEach(task => bootstrapper.Tasks.TryAddTask(task));

            // execute initialization tasks
            var container = bootstrapper.Initialize();

            // expose container
            if (this.exposeContainer != null)
            {
                this.exposeContainer(container);
            }

            // start
            bootstrapper.Start();

            return bootstrapper;
        }

        public ISetupBootstrapper Task(IInitializationTask task)
        {
            this.taskList.Add(task);

            return this;
        }
    }
}