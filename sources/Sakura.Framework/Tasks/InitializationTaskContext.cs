namespace Sakura.Framework.Tasks
{
    using System.Collections.Generic;

    using Autofac;

    using Sakura.Framework.Registration;

    public class InitializationTaskContext
    {
        public InitializationTaskContext(ContainerBuilder builder, IEnumerable<IRegistrationPolicy> policies)
        {
            this.Builder = builder;
            this.Policies = policies;
        }

        public ContainerBuilder Builder { get; private set; }

        public IEnumerable<IRegistrationPolicy> Policies { get; private set; }
    }
}