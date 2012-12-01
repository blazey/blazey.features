using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Machine.Specifications;

namespace blazey.features.specs
{
    internal class when_component_is_an_unreleased_feature
    {
        private static IFeature _resolvedFeature;
        private static IWindsorContainer _container;
        private static Exception _exception;

        private Establish that_windsor_is_configured = () =>
            {
                _container = new WindsorContainer();
                _container.Kernel.Resolver.AddSubResolver(new FeatureResolver(_container.Kernel));
                _container.Register(AllTypes.FromThisAssembly().BasedOn<IFeature>().WithServiceAllInterfaces());
                _container.Register(
                    Component.For<Service>(),
                    Component.For<IFeature>().ImplementedBy<ReleasedFeature>(),
                    Component.For<IFeatureSpecification<IFeature>>().ImplementedBy<FeatureSpecification>());

              
            };

        private Because windsor_resolves = () => _exception = Catch.Exception(
            () => _resolvedFeature = _container.Resolve<Service>().Feature);

        private It should_not_throw = () => _exception.ShouldBeNull();
        private It should_resolve_as_unreleased = () => _resolvedFeature.ShouldBeOfType<UnreleasedFeature>();
    }
}