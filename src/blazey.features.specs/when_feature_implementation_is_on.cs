using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Machine.Specifications;
using blazey.features.specs.Doubles;

namespace blazey.features.specs
{
    [Subject(typeof(FeaturesFacility))]
    internal class when_feature_implementation_is_on
    {

        private Establish that_windsor_is_configured = () =>
        {
            _windsorContainer = new WindsorContainer();

            _windsorContainer.AddFacility(FeaturesFacility.RegisterFeatureSpecifications(_windsorContainer, register =>
                                   register.AddFeatueSpecification<DummyFeatureSpecification, ISomeFeature>()));
    
            _windsorContainer.Register(Component.For<ISomeFeature>().ImplementedBy<ReleasedFeature>(),
                                       Component.For<FeatureTableService>());
        };

        private Because windsor_resolves = () => _exception = Catch.Exception(
            () => _isOn = _windsorContainer.Resolve<FeatureTableService>().IsOn<ISomeFeature, UnreleasedFeature>());

        private It should_not_throw = () => _exception.ShouldBeNull();
        private It should_be_on = () => _isOn.ShouldBeTrue();

        private static bool _isOn;
        private static Exception _exception;
        private static WindsorContainer _windsorContainer;
        
    }
}

