namespace Sakura.Framework.Registration
{
    using System;

    using Autofac;

    public interface IRegistrationPolicy
    {
        bool IsMatch(Type type);

        void Apply(Type dependencyType, ContainerBuilder builder);
    }
}