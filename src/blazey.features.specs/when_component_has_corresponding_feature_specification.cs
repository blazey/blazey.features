using System;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Machine.Specifications;
using blazey.features.specs.Doubles;
using blazey.features.specs.configuration;

namespace blazey.features.specs
{
    [Subject(typeof(FeaturesFacility))]
    public class when_component_has_corresponding_feature_specification
    {
        private static object _resolvedFeature;
        private static Exception _exception;
        private static readonly Features _features = new Features();

        public Establish that_component_has_feature_specifcation = _features.EstablishWindsor(config =>
        {
            config.AddFeatueSpecification<DummyFeatureSpecification, ISomeFeature>();
            config.RegisterComponent(Component.For<ISomeFeature>().ImplementedBy<ReleasedFeature>());
        });

        public Because windsor_resolves = () => _exception = Catch.Exception(
            () => _resolvedFeature = _features.ResolveFeature());

        public It should_not_throw = () => _exception.ShouldBeNull();
        public It should_resolve_as_unreleased = () => _resolvedFeature.ShouldBeOfType<UnreleasedFeature>();
    }
}