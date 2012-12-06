using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Machine.Specifications;
using blazey.features.specs.Doubles;

namespace blazey.features.specs.configuration
{
    public class Features
    {

        private IWindsorContainer _container;

        internal Establish ConfigureWindsor(Action<FeaturesUnitTestConfiguration> register)
        {

            var unitTestConfiguration = new FeaturesUnitTestConfiguration();
            _container = unitTestConfiguration.Container;
            _container.Register(Component.For<Service>());
            register(unitTestConfiguration);
            unitTestConfiguration.ConfigureWindsor(_container);

            return () => { };
        }

        internal object ResolveFeature()
        {
            return _container.Resolve<Service>().Feature;
        }

    }
}