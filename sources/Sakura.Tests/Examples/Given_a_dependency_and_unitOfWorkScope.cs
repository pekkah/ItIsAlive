namespace Sakura.Framework.Tests.Examples
{
    using Autofac;

    using Machine.Fakes;
    using Machine.Specifications;

    using Sakura.Bootstrapping.Setup;
    using Sakura.Framework.Dependencies.DefaultTypes;

    public interface IMyUnitOfWorkDepedency : IUnitOfWorkDependency
    {
    }

    public class UnitOfWorkDependency : IMyUnitOfWorkDepedency
    {
    }

    public class Given_a_dependency_and_unitOfWorkScope : WithSubject<Setup>
    {
        private static IContainer container;

        private Establish context = () => Subject.Dependencies(from => from.Types(typeof(UnitOfWorkDependency)));

        private Because of = () => Subject.ExposeContainer(exposed => container = exposed).Start();

        private It should_resolve_dependency_from_scope = () =>
            {
                using (ILifetimeScope unitofWorkScope = container.BeginLifetimeScope("unitOfWork"))
                {
                    var unitOfWorkDependency = unitofWorkScope.Resolve<IMyUnitOfWorkDepedency>();

                    unitOfWorkDependency.ShouldNotBeNull();
                    unitOfWorkDependency.ShouldBeOfType<IMyUnitOfWorkDepedency>();
                }
            };
    }
}