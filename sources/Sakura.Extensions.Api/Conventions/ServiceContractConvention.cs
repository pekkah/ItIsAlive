﻿namespace Sakura.Extensions.Api.Conventions
{
    using System;
    using System.ServiceModel;

    using Autofac;

    using Sakura.Framework.Dependencies.Conventions;

    public class ServiceContractConvention : IRegistrationConvention
    {
        public void Apply(Type dependencyType, ContainerBuilder builder)
        {
            builder.RegisterType(dependencyType).AsSelf().InstancePerLifetimeScope();
        }

        public bool IsMatch(Type type)
        {
            return Attribute.IsDefined(type, typeof(ServiceContractAttribute), true);
        }
    }
}