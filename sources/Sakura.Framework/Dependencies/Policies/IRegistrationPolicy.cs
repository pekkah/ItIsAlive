namespace Sakura.Framework.Dependencies.Policies
{
    using System;

    using Autofac;

    public interface IRegistrationPolicy
    {
        void Apply(Type dependencyType, ContainerBuilder builder);

        bool IsMatch(Type type);
    }
}