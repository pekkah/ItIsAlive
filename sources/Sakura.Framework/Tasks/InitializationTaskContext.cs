namespace Fugu.Framework.Tasks
{
    using Autofac;

    public class InitializationTaskContext
    {
        public ContainerBuilder Builder { get; private set; }

        public InitializationTaskContext(ContainerBuilder builder)
        {
            this.Builder = builder;
        }
    }
}