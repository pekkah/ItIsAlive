namespace Sakura.Composition
{
    using System;

    using Autofac;

    public interface IRegistrationConvention
    {
        void Apply(Type dependencyType, ContainerBuilder builder);

        bool IsMatch(Type type);
    }
}