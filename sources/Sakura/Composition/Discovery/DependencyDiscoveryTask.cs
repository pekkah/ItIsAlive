namespace Sakura.Composition.Discovery
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Autofac;

    using Sakura.Bootstrapping.Tasks;

    public class DependencyDiscoveryTask : IInitializationTask
    {
        private readonly List<IRegistrationConvention> conventions;

        private readonly IDependencyLocator locator;

        public DependencyDiscoveryTask(IDependencyLocator locator)
        {
            this.locator = locator;
            this.conventions = new List<IRegistrationConvention>();
        }

        public IEnumerable<IRegistrationConvention> Conventions
        {
            get
            {
                return this.conventions;
            }
        }

        public void AddConvention(IRegistrationConvention convention)
        {
            this.conventions.Add(convention);
        }

        public void Execute(InitializationTaskContext context)
        {
            var matchingTypes = this.locator.GetDependencies(this.conventions);

            foreach (var matchingType in matchingTypes)
            {
                this.Register(context, matchingType);
            }
        }

        public void RemoveConvention(IRegistrationConvention convention)
        {
            this.conventions.Remove(convention);
        }

        private void Register(InitializationTaskContext context, Type matchingType)
        {
            var registration = context.Builder.RegisterType(matchingType);
            foreach (var policy in this.conventions.Where(p => p.IsMatch(matchingType)))
            {
                policy.Apply(registration, matchingType);
            }
        }
    }
}