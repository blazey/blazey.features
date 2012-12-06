using System;
using Castle.MicroKernel.Registration;
using Machine.Specifications;
using blazey.features.specs.Doubles;

namespace blazey.features.specs
{
    [Ignore]
    internal class when_feature_specifcation_is_registered_with_contract_not_concrete_type
    {
        private static object _resolvedFeature;
        private static Exception _exception;
        private static readonly Features _features = new Features();

        private Establish that_windsor_is_configured = _features.ConfigureWindsor(config =>
            {
                config.AddFeatueSpecification<IFeatureSpecification<ISomeFeature>, ISomeFeature>();
                config.RegisterComponent(Component.For<ISomeFeature>().ImplementedBy<ReleasedFeature>());
            });

        private Because windsor_resolves = () => _exception = Catch.Exception(
            () => _resolvedFeature = _features.ResolveFeature());

        private It should_throw_invlaid_operation = () => _exception.ShouldBeOfType<InvalidOperationException>();
    }
}