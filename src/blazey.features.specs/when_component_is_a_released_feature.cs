using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Machine.Specifications;

namespace blazey.features.specs
{
    internal class when_component_is_a_released_feature
    {
        private static IFeature _resolvedFeature;
        private static IWindsorContainer _container;
        private static Exception _exception;
        private It should_not_throw = () => _exception.ShouldBeNull();
        private It should_resolve_as_released = () => _resolvedFeature.ShouldBeOfType<ReleasedFeature>();

        private Establish that_windsor_is_configured = () =>
            {
                _container = new WindsorContainer();
                _container.Register(Component.For<IFeature>().ImplementedBy<ReleasedFeature>());
            };

        private Because windsor_resolves = () => _exception = Catch.Exception(
            () => _resolvedFeature = _container.Resolve<IFeature>());
    }
}