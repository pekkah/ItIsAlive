namespace Sakura.Extensions.Mvc.Conventions
{
    using System;
    using System.Web.Mvc;

    using Autofac;

    using Sakura.Bootstrapping;
    using Sakura.Composition;

    public class ControllersAsSelf : IRegistrationConvention
    {
        private readonly AsSelfConvention asSelfConvention;

        public ControllersAsSelf()
        {
            this.asSelfConvention = new AsSelfConvention();
        }

        public void Apply(Type dependencyType, ContainerBuilder builder)
        {
            this.asSelfConvention.Apply(dependencyType, builder);
        }

        public bool IsMatch(Type type)
        {
            return typeof(IController).IsAssignableFrom(type);
        }
    }
}