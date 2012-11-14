namespace Bootstrapper.Bootstrapping.Tasks
{
    using Autofac;

    public class InitializationTaskContext
    {
        public InitializationTaskContext(ContainerBuilder builder)
        {
            Builder = builder;
        }

        public ContainerBuilder Builder { get; private set; }
    }
}