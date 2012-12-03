using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Machine.Specifications;
using blazey.features.specs.Doubles;

namespace blazey.features.specs
{
    internal class when_component_is_an_unreleased_feature
    {
        private static IFeature _resolvedFeature;
        private static Exception _exception;

        private Establish that_windsor_is_configured = Features.ConfigureWindsor(config => {
                          config.AddImplementation()
            });

        private Because windsor_resolves = () => _exception = Catch.Exception(
            () => _resolvedFeature = Features.Resolve<Service>().Feature);

        private It should_not_throw = () => _exception.ShouldBeNull();
        private It should_resolve_as_unreleased = () => _resolvedFeature.ShouldBeOfType<UnreleasedFeature>();
    }

    internal class FeaturesConfiguration
    {
        readonly List<Tuple<Type, Type>> _services = new List<Tuple<Type, Type>>();

        internal void AddImplementation<TFeature>(TFeature service, TFeature implementation)
        {
            _services.Add(new Tuple<Type, Type>(service.GetType(), implementation.GetType()));
        }
    }

    internal class Features
    {
        public static IWindsorContainer Container { get; private set; }

        protected Features()
        {
            Container = new WindsorContainer();
        }

        internal static Establish ConfigureWindsor(Action<FeaturesConfiguration> config)
        {
            return () =>
                {
                    Container = new WindsorContainer();
                    Container.Kernel.Resolver.AddSubResolver(new FeatureResolver(Container.Kernel));
                    Container.Register(
                        Component.For<Service>(),
                        Component.For<IFeature>().ImplementedBy<ReleasedFeature>(),
                        Component.For<IFeatureSpecification<IFeature>>().ImplementedBy<DummyFeatureSpecification>());
                };
        }

        internal static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

    }
}