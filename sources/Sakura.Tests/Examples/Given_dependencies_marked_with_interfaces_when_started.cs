namespace Sakura.Framework.Tests.Examples
{
    using Autofac;

    using Machine.Fakes;
    using Machine.Specifications;

    using Sakura.Bootstrapping.Setup;
    using Sakura.Framework.Dependencies.DefaultTypes;

    public interface IMyTransientDependency : ITransientDependency
    {     
    }

    public interface IMySingleInstanceDependency : ISingleInstanceDependency
    {    
    }

    public class MyTransientDependency : IMyTransientDependency
    {
    }

    public class MySingleInstanceDependency : IMySingleInstanceDependency
    {
    }

    public class Given_dependencies_marked_with_interfaces_when_started : WithSubject<Setup>
    {
        private static IContainer container;

        private Establish context = () => Subject
            .Dependencies(from => from.Types(typeof(MyTransientDependency), typeof(MySingleInstanceDependency)));

        private Because of = () => Subject.ExposeContainer(exposed => container = exposed).Start();

        private It should_register_dependencies_by_implemented_interfaces = () =>
            {
                var myTransientDependency = container.Resolve<IMyTransientDependency>();
                var mySingleInstanceDependency = container.Resolve<IMySingleInstanceDependency>();

                myTransientDependency.ShouldNotBeNull();
                myTransientDependency.ShouldBeOfType<MyTransientDependency>();

                mySingleInstanceDependency.ShouldNotBeNull();
                mySingleInstanceDependency.ShouldBeOfType<MySingleInstanceDependency>();
            };
    }

    public class Given_dependencies_marked_with_interfaces_when_started_alt : WithSubject<Setup>
    {
        private static IContainer container;

        private Establish context = () => Subject.Dependencies(from => from.AssemblyOf<MySingleInstanceDependency>());

        private Because of = () => Subject.ExposeContainer(exposed => container = exposed).Start();

        private It should_register_dependencies_by_implemented_interfaces = () =>
        {
            var myTransientDependency = container.Resolve<IMyTransientDependency>();
            var mySingleInstanceDependency = container.Resolve<IMySingleInstanceDependency>();

            myTransientDependency.ShouldNotBeNull();
            myTransientDependency.ShouldBeOfType<MyTransientDependency>();

            mySingleInstanceDependency.ShouldNotBeNull();
            mySingleInstanceDependency.ShouldBeOfType<MySingleInstanceDependency>();
        };
    }
}