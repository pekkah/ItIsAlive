namespace Sakura.Framework.Tasks
{
    using Autofac;

    public class InitializationTaskContext
    {
        public InitializationTaskContext(ContainerBuilder builder)
        {
            this.Builder = builder;
        }

        public ContainerBuilder Builder { get; private set; }
    }
}