namespace Sakura.Extensions.Mvc.Conventions
{
    using System;
    using System.Web.Mvc;

    using Autofac.Builder;

    using Sakura.Composition;
    using Sakura.Composition.Conventions;

    public class ControllersAsSelf : IRegistrationConvention
    {
        private readonly AsSelfConvention asSelfConvention;

        public ControllersAsSelf()
        {
            this.asSelfConvention = new AsSelfConvention();
        }

        public void Apply(
            IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration,
            Type dependencyType)
        {
            this.asSelfConvention.Apply(registration, dependencyType);
        }

        public bool IsMatch(Type type)
        {
            return typeof(IController).IsAssignableFrom(type);
        }
    }
}