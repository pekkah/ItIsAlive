namespace ItIsAlive.Tasks
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