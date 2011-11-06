namespace Sakura.Extensions.Mvc.Policies
{
    using System;
    using System.Web.Mvc;

    using Autofac;

    using Sakura.Framework.Registration;

    public class ControllersAsSelf : IRegistrationPolicy
    {
        private SelfPolicy selfPolicy;

        public ControllersAsSelf()
        {
            this.selfPolicy = new SelfPolicy();
        }

        public bool IsMatch(Type type)
        {
            return typeof(IController).IsAssignableFrom(type);
        }

        public void Apply(Type dependencyType, ContainerBuilder builder)
        {
            this.selfPolicy.Apply(dependencyType, builder);
        }
    }
}