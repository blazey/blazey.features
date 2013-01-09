using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Machine.Specifications;
using blazey.features.specs.doubles;

namespace blazey.features.specs
{
    [Subject(typeof (FeaturesTable))]
    internal class when_features_are_reconciled_against_specifications
    {
        private Establish context = () =>
            {
                var container = new WindsorContainer();

                container.AddFacility(
                    FeaturesFacility.RegisterFeatureSpecifications(
                        container, register =>
                            {
                                register.AddFeatueSpecification<FeatureSpecificationX, FeatureX>();
                                register.AddFeatueSpecification<FeatureSpecificationY, FeatureY>();
                                register.AddFeatueSpecification<FeatureSpecificationZ, FeatureZ>();
                            }));

                container.Register(Component.For<FeatureZ>().ImplementedBy<FeatureZ>(),
                                   Component.For<FeatureTableService>());

                _featureTableService = container.Resolve<FeatureTableService>();
            };

        private Because specifications_missing_registered_features =
            () => _exception = Catch.Exception(() => _featureTableService.ValidateSpecificiedFeaturesAreRegistered());

        private It should_throw_un_registered_feature_is_specificied_exception
            = () => _exception.ShouldBeOfType<UnRegisteredFeatureIsSpecifiedException>();

        private It should_list_unregistered_features_containing_specification_x_and_feature_x
            = () => ResolvesInvalidSpecification<FeatureSpecificationX, FeatureX>(_exception);

        private It should_list_unregistered_features_containing_specification_y_and_feature_ =
            () => ResolvesInvalidSpecification<FeatureSpecificationY, FeatureY>(_exception);

        private It should_not_list_unregistered_features_containing_specification_z_and_feature_z
            = () => ((UnRegisteredFeatureIsSpecifiedException) _exception)
                        .InvalidSpecifications.ContainsKey(typeof (FeatureSpecificationZ)).ShouldBeFalse();

        private static It ResolvesInvalidSpecification<TFeatureSpecification, TFeature>(Exception exception)
        {
            var unRegisteredFeatureIsSpecifiedException = (UnRegisteredFeatureIsSpecifiedException) exception;

            unRegisteredFeatureIsSpecifiedException
                .InvalidSpecifications[typeof (TFeatureSpecification)]
                .ShouldEqual(typeof (TFeature));

            return () => { };
        }

        private static Exception _exception;
        private static FeatureTableService _featureTableService;
    }
}