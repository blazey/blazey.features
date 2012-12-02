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

        private Establish that_windsor_is_configured = Features.ConfigureWindsor();

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

        protected Features(Action<FeaturesConfiguration> config)
        {
            Container = new WindsorContainer();
        }

        internal static Establish ConfigureWindsor()
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

    internal class when_component_is_implemented_as_null
    {
        private static IFeature _resolvedFeature;
        private static IWindsorContainer _container;
        private static Exception _exception;

        private Establish that_windsor_is_configured = () =>
        {
            _container = new WindsorContainer();
            _container.Kernel.Resolver.AddSubResolver(new FeatureResolver(_container.Kernel));
            _container.Register(
                Component.For<Service>(),
                Component.For<IFeature>().ImplementedBy<ReleasedFeature>(),
                Component.For<IFeatureSpecification<IFeature>>().ImplementedBy<DummyFeatureSpecification>());
            };

        private Because windsor_resolves = () => _exception = Catch.Exception(
            () => _resolvedFeature = _container.Resolve<Service>().Feature);

        private It should_not_throw = () => _exception.ShouldBeNull();
        private It should_resolve_as_unreleased = () => _resolvedFeature.ShouldBeOfType<UnreleasedFeature>();
    }

}