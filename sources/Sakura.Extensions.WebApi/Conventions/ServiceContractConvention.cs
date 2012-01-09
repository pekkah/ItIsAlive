namespace Sakura.Extensions.WebApi.Conventions
{
    using System;
    using System.ServiceModel;

    using Autofac;
    using Autofac.Builder;

    using Sakura.Composition;

    public class ServiceContractConvention : IRegistrationConvention
    {
        public void Apply(
            IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration,
            Type dependencyType)
        {
            registration.AsSelf().InstancePerLifetimeScope();
        }

        public bool IsMatch(Type type)
        {
            return Attribute.IsDefined(type, typeof(ServiceContractAttribute), true);
        }
    }
}