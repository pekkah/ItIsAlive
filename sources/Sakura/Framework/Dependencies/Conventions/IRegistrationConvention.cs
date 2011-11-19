namespace Sakura.Framework.Dependencies.Conventions
{
    using System;

    using Autofac;

    public interface IRegistrationConvention
    {
        void Apply(Type dependencyType, ContainerBuilder builder);

        bool IsMatch(Type type);
    }
}