using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Machine.Specifications;
using blazey.features.configuration;
using blazey.features.specs.Doubles;

namespace blazey.features.specs
{
    public class Features
    {

        private IWindsorContainer _container;

        internal Establish ConfigureWindsor(Action<FeaturesUnitTestConfiguration> config)
        {

            var unitTestConfiguration = new FeaturesUnitTestConfiguration();
            _container = unitTestConfiguration.Container;
            _container.Register(Component.For<Service>());
            config(unitTestConfiguration);
            unitTestConfiguration.ConfigureWindsor(_container);

            return () => { };
        }

        internal object ResolveFeature()
        {
            return _container.Resolve<Service>().Feature;
        }

    }

    internal class FeaturesUnitTestConfiguration : FeaturesConfiguration
    {
        public FeaturesUnitTestConfiguration()
        {
            Container = new WindsorContainer();
        }

        public IWindsorContainer Container { get; private set; }

        internal void RegisterComponent(params IRegistration[] registration)
        {
            Container.Register(registration);
        }
    }
}