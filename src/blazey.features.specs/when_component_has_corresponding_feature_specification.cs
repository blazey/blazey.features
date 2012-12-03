using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Machine.Specifications;
using blazey.features.specs.Doubles;

namespace blazey.features.specs
{
    internal class when_component_has_corresponding_feature_specification
    {
        private static IFeature _resolvedFeature;
        private static IWindsorContainer _container;
        private static Exception _exception;

        private Establish that_windsor_is_configured = () =>
            {
                _container = new WindsorContainer();
                _container.AddFacility<FeatureFacility>();
                _container.Register(
                    Component.For<Service>(),
                    Component.For<IFeature>().ImplementedBy<ReleasedFeature>());
                
                FeatureFacility.AddFeatueSpecification<DummyFeatureSpecification, ReleasedFeature>(_container);

               };

        private Because windsor_resolves = () => _exception = Catch.Exception(
            () => _resolvedFeature = _container.Resolve<Service>().Feature);

        private It should_not_throw = () => _exception.ShouldBeNull();
        private It should_resolve_as_unreleased = () => _resolvedFeature.ShouldBeOfType<UnreleasedFeature>();
    }

}