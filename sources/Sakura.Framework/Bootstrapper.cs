namespace Sakura.Framework
{
    using Sakura.Framework.Registration;

    public class Bootstrapper : AbstractBootstrapper
    {
        public Bootstrapper()
        {
            this.Policies.Add(new SelfPolicy());
            this.Policies.Add(new SingleInstancePolicy());
            this.Policies.Add(new TransientPolicy());
        }
    }
}