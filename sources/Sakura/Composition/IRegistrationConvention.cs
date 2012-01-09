namespace Sakura.Composition
{
    using System;

    using Autofac;
    using Autofac.Builder;

    public interface IRegistrationConvention
    {
        void Apply(IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration, Type dependencyType);

        bool IsMatch(Type type);
    }
}