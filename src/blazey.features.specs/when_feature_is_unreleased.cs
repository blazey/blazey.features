using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Machine.Specifications;
using blazey.features.specs.Doubles;

namespace blazey.features.specs
{
    [Subject(typeof(FeaturesFacility))]
    internal class when_feature_is_unreleased
    {

        private Establish that_windsor_is_configured = () =>
            {
                _windsorContainer = new WindsorContainer();
                _windsorContainer.AddFacility(FeaturesFacility.RegisterFeatureSpecifications(
                    _windsorContainer, register =>
                                       register.AddFeatueSpecification<DummyFeatureSpecification, ISomeFeature>()));

                _windsorContainer.Register(Component.For<ISomeFeature>().ImplementedBy<ReleasedFeature>(),
                                           Component.For<ServiceWithAFeature>());
            };

        private Because windsor_resolves = () => _exception = Catch.Exception(
            () => _resolvedFeature = _windsorContainer.Resolve<ServiceWithAFeature>().Feature);

        private It should_not_throw = () => _exception.ShouldBeNull();
        private It should_resolve_as_unreleased = () => _resolvedFeature.ShouldBeOfType<UnreleasedFeature>();

        private static object _resolvedFeature;
        private static Exception _exception;
        private static WindsorContainer _windsorContainer;

    }
}