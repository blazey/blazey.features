using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using blazey.features.configuration;

namespace blazey.features.specs.configuration
{
    internal class FeaturesUnitTestConfiguration
    {
        internal FeaturesUnitTestConfiguration()
        {
            Container = new WindsorContainer();
        }

        internal void Configuration(Action<FeaturesConfiguration> featuresConfig)
        {
            var config = new FeaturesConfiguration();
            featuresConfig(config);
            config.ConfigureWindsor(Container);

        }

        public IWindsorContainer Container { get; private set; }

        internal void RegisterComponent(params IRegistration[] registration)
        {
            Container.Register(registration);
        }
    }
}