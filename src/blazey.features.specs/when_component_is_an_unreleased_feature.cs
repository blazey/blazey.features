using System;
using Machine.Specifications;
using blazey.features.specs.Doubles;

namespace blazey.features.specs
{
    internal class when_component_is_an_unreleased_feature
    {
        private static ISomeFeature _resolvedFeature;
        private static Exception _exception;
        private static readonly Features _features = new Features();

        private Establish that_windsor_is_configured = _features.ConfigureWindsor(config =>
            {
                config.AddFeatueSpecification<DummyFeatureSpecification, ISomeFeature>();
                config.AddFeature<ISomeFeature, ReleasedFeature>();
            });

        private Because windsor_resolves = () => _exception = Catch.Exception(
            () => _resolvedFeature = _features.Resolve<Service>().Feature);

        private It should_not_throw = () => _exception.ShouldBeNull();
        private It should_resolve_as_unreleased = () => _resolvedFeature.ShouldBeOfType<UnreleasedFeature>();
    }
}