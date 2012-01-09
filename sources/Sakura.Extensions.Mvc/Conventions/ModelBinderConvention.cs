namespace Sakura.Extensions.Mvc.Conventions
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using Autofac;
    using Autofac.Builder;
    using Autofac.Integration.Mvc;

    using Sakura.Composition;

    public class ModelBinderConvention : IRegistrationConvention
    {
        public void Apply(
            IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration,
            Type dependencyType)
        {
            registration.As<IModelBinder>().InstancePerLifetimeScope().WithMetadata(
                "SupportedModelTypes", this.GetTargetType(dependencyType)).PropertiesAutowired(
                    PropertyWiringFlags.PreserveSetValues);
        }

        public bool IsMatch(Type type)
        {
            return typeof(IModelBinder).IsAssignableFrom(type);
        }

        private object GetTargetType(Type type)
        {
            return
                (from ModelBinderTypeAttribute attribute in
                     type.GetCustomAttributes(typeof(ModelBinderTypeAttribute), true)
                 from targetType in attribute.TargetTypes
                 select targetType).ToList();
        }
    }
}