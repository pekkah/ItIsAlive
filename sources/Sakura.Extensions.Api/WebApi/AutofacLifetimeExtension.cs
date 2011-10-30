namespace Sakura.Extensions.Api.WebApi
{
    using System;
    using System.ServiceModel;

    using Autofac;

    public class AutofacLifetimeExtension : IExtension<InstanceContext>, IDisposable
    {
        private readonly ILifetimeScope lifetimeScope;

        private bool isDisposed;

        public AutofacLifetimeExtension(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
        }

        public void Attach(InstanceContext owner)
        {
        }

        public void Detach(InstanceContext owner)
        {
            this.Dispose();
        }

        public void Dispose()
        {
            if (this.isDisposed)
            {
                return;
            }

            this.lifetimeScope.Dispose();
            this.isDisposed = true;
        }
    }
}