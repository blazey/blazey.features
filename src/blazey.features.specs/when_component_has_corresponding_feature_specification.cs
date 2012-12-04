using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Machine.Specifications;
using blazey.features.specs.Doubles;

namespace blazey.features.specs
{
    [Subject(typeof(FeaturesFacility))]
    public class when_component_has_corresponding_feature_specification
    {
        private static ISomeFeature _resolvedFeature;
        private static IWindsorContainer _container;
        private static Exception _exception;
        private static readonly Features _features = new Features();

        public Establish that_windsor_is_configured = _features.ConfigureWindsor(config =>
            {
                config.AddFeatueSpecification<IFeatureSpecification<ReleasedFeature>, ISomeFeature>();
            }); 
        /*() =>
            {
                _container = new WindsorContainer();
                _container.AddFacility<FeaturesFacility>();
                _container.Register(
                    Component.For<Service>(),
                    Component.For<ISomeFeature>().ImplementedBy<ReleasedFeature>());

    };*/

        public Because windsor_resolves = () => _exception = Catch.Exception(
            () => _resolvedFeature = _container.Resolve<Service>().Feature);

        public It should_not_throw = () => _exception.ShouldBeNull();
        public It should_resolve_as_unreleased = () => _resolvedFeature.ShouldBeOfType<UnreleasedFeature>();
    }

}