namespace Sakura.Bootstrapping
{
    using System;
    using System.Collections.Generic;

    using Autofac;

    using Sakura.Bootstrapping.Tasks;
    using Sakura.Composition;

    public class Configure : IConfigureBootstrapper
    {
        private readonly Bootstrapper bootstrapper;

        private Action<IContainer> exposeContainer;

        private readonly ConfigureDependencies configureDependencies;

        public Configure()
        {
            this.bootstrapper = new Bootstrapper();
            this.configureDependencies = new ConfigureDependencies();
        }

        public IConfigureBootstrapper Dependencies(Action<IConfigureDependencies> dependencies)
        {
            if (dependencies == null)
            {
                throw new ArgumentNullException("dependencies");
            }

            dependencies(this.configureDependencies);

            return this;
        }

        public IConfigureBootstrapper Conventions(Action<ISet<IRegistrationConvention>> conventions)
        {
            if (conventions == null)
            {
                throw new ArgumentNullException("conventions");
            }

            // conventions(this.bootstrapper.Conventions);

            return this;
        }

        public IConfigureBootstrapper ExposeContainer(Action<IContainer> exposeTo)
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
            // execute initialization tasks
            var container = this.bootstrapper.Initialize();

            // expose container
            if (this.exposeContainer != null)
            {
                this.exposeContainer(container);
            }

            // start
            this.bootstrapper.Start();

            return this.bootstrapper;
        }

        public IConfigureBootstrapper Tasks(Action<IList<IInitializationTask>> modifyTasks)
        {
            if (modifyTasks == null)
            {
                throw new ArgumentNullException("modifyTasks");
            }

            modifyTasks(this.bootstrapper.Tasks);

            return this;
        }
    }
}