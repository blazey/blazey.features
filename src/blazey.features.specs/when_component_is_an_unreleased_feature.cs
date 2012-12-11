using System;
using Castle.MicroKernel.Registration;
using Machine.Specifications;
using blazey.features.specs.Doubles;
using blazey.features.specs.configuration;

namespace blazey.features.specs
{
    [Subject(typeof(FeaturesFacility))]
    internal class when_component_is_an_unreleased_feature
    {
        private static object _resolvedFeature;
        private static Exception _exception;
        private static readonly Features _features = new Features();

        private Establish that_windsor_is_configured = _features.EstablishWindsor(config =>
            {
                config.AddFeatueSpecification<DummyFeatureSpecification, ISomeFeature>();
                config.RegisterComponent(Component.For<ISomeFeature>().ImplementedBy<ReleasedFeature>());
            });

        private Because windsor_resolves = () => _exception = Catch.Exception(
            () => _resolvedFeature = _features.ResolveFeature());

        private It should_not_throw = () => _exception.ShouldBeNull();
        private It should_resolve_as_unreleased = () => _resolvedFeature.ShouldBeOfType<UnreleasedFeature>();
    }
}

