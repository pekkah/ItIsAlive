namespace Sakura.Extensions.NHibernateMvc
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using Autofac;
    using Autofac.Integration.Mvc;

    using Sakura.Extensions.NHibernateMvc.Filters;
    using Sakura.Framework.Dependencies.Policies;
    using Sakura.Framework.Fluent;

    public static class SetupBootExtensions
    {
        public static ISetupBootstrapper ActivateNHibernateMvcIntegration(this ISetupBootstrapper setup)
        {
            return setup.Dependencies(d => d.AssemblyOf<WorkContextBinder>()).RegistrationPolicies(policies => policies.Add(new ModelBinderPolicy()));
        }
    }

    public class ModelBinderPolicy : IRegistrationPolicy
    {
        public void Apply(Type dependencyType, ContainerBuilder builder)
        {
            builder.RegisterType(dependencyType).As<IModelBinder>().InstancePerHttpRequest().WithMetadata(
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