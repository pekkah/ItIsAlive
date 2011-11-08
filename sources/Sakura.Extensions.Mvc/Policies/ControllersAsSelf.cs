namespace Sakura.Extensions.Mvc.Policies
{
    using System;
    using System.Web.Mvc;

    using Autofac;

    using Sakura.Framework.Dependencies.Policies;

    public class ControllersAsSelf : IRegistrationPolicy
    {
        private AsSelfPolicy asSelfPolicy;

        public ControllersAsSelf()
        {
            this.asSelfPolicy = new AsSelfPolicy();
        }

        public bool IsMatch(Type type)
        {
            return typeof(IController).IsAssignableFrom(type);
        }

        public void Apply(Type dependencyType, ContainerBuilder builder)
        {
            this.asSelfPolicy.Apply(dependencyType, builder);
        }
    }
}