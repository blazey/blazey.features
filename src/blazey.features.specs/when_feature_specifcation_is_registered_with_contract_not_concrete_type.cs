using System;
using Machine.Specifications;
using blazey.features.configuration;
using blazey.features.specs.Doubles;

namespace blazey.features.specs
{
    [Subject(typeof(FeaturesConfiguration))]
    internal class when_feature_specifcation_is_registered_with_contract_not_concrete_type
    {
        private static Exception _exception;
        private static  FeaturesConfiguration _featuresConfiguration = new FeaturesConfiguration();

        private Establish that_windsor_is_configured = () => _featuresConfiguration = new FeaturesConfiguration();

        private Because windsor_resolves = () => _exception = Catch.Exception(
            () => _featuresConfiguration.AddFeatueSpecification<IFeatureSpecification<ISomeFeature>, ISomeFeature>());

        private It should_throw_invlaid_operation = () => _exception.ShouldBeOfType<InvalidOperationException>();
    }
}