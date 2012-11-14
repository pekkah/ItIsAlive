namespace ItIsAlive.Composition.Discovery
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Autofac;

    using Bootstrapping.Tasks;

    public class DependencyDiscoveryTask : IInitializationTask
    {
        private readonly List<IRegistrationConvention> conventions;

        private readonly IDependencyLocator locator;

        public DependencyDiscoveryTask(IDependencyLocator locator)
        {
            this.locator = locator;
            conventions = new List<IRegistrationConvention>();
        }

        public IEnumerable<IRegistrationConvention> Conventions
        {
            get
            {
                return conventions;
            }
        }

        public void Execute(InitializationTaskContext context)
        {
            var matchingTypes = locator.GetDependencies(conventions);

            foreach (var matchingType in matchingTypes)
            {
                Register(context, matchingType);
            }
        }

        public void AddConvention(IRegistrationConvention convention)
        {
            conventions.Add(convention);
        }

        public void RemoveConvention(IRegistrationConvention convention)
        {
            conventions.Remove(convention);
        }

        private void Register(InitializationTaskContext context, Type matchingType)
        {
            var registration = context.Builder.RegisterType(matchingType);
            foreach (var policy in conventions.Where(p => p.IsMatch(matchingType)))
            {
                policy.Apply(registration, matchingType);
            }
        }
    }
}