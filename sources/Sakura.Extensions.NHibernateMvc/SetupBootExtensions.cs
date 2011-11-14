﻿namespace Sakura.Extensions.NHibernateMvc
{
    using Sakura.Bootstrapping.Setup;
    using Sakura.Extensions.NHibernate;
    using Sakura.Extensions.NHibernateMvc.Binders;
    using Sakura.Extensions.NHibernateMvc.Filters;

    public static class SetupBootExtensions
    {
        public static ISetupBootstrapper EnableWorkContext(this ISetupBootstrapper setup)
        {
            return setup.Dependencies(d => d.Types(typeof(WorkContextBinder), typeof(WorkContextTransactionAttribute))).
                    Tasks(manager => manager.ReplaceTask<RegisterSession>(new OverrideSessionRegistration()));
        }
    }
}