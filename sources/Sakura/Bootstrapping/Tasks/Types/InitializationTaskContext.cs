namespace Sakura.Bootstrapping.Tasks.Types
{
    using System.Collections.Generic;

    using Autofac;

    using Sakura.Framework.Dependencies.Conventions;

    public class InitializationTaskContext
    {
        public InitializationTaskContext(ContainerBuilder builder, IEnumerable<IRegistrationConvention> policies)
        {
            this.Builder = builder;
            this.Conventions = policies;
        }

        public ContainerBuilder Builder { get; private set; }

        public IEnumerable<IRegistrationConvention> Conventions { get; private set; }
    }
}