namespace Sakura.TestHelpers
{
    using System;

    using Autofac;

    using Sakura.Bootstrapping.Tasks;

    public class RegisterDependency : IInitializationTask
    {
        private readonly Action<ContainerBuilder> register;

        public RegisterDependency(Action<ContainerBuilder> register)
        {
            this.register = register;
        }

        public void Execute(InitializationTaskContext context)
        {
            this.register(context.Builder);
        }
    }
}