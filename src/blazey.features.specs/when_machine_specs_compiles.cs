using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Machine.Specifications;

namespace blazey.features.specs
{
    [Subject(typeof(when_machine_specs_compiles), "If mspec runner is working and test compiles, this test will always pass.")]
    class when_machine_specs_compiles
    {
        private It should_compile = () => true.ShouldBeTrue();
    }

    class when_component_is_a_released_feature
    {
        private Establish that_windsor_is_configured = () =>
            {
                _container = new WindsorContainer();
                _container.Register(Component.For<IFeature>().ImplementedBy<ReleasedFeature>());
            };

        private Because windsor_resolves = () => _exception = Catch.Exception(
            ()=> _resolvedFeature = _container.Resolve<IFeature>());

        private It should_resolve_as_released = () => _resolvedFeature.ShouldBeOfType<ReleasedFeature>();
        private It should_not_throw = () => _exception.ShouldBeNull();

        private static IFeature _resolvedFeature;
        private static IWindsorContainer _container;
        private static Exception _exception;
    }

    class when_component_is_an_unreleased_feature
    {
        private Establish that_windsor_is_configured = () =>
        {
            _container = new WindsorContainer();
            _container.Register(Component.For<IFeature>().ImplementedBy<ReleasedFeature>());
        };

        private Because windsor_resolves = () => _exception = Catch.Exception(
            () => _resolvedFeature = _container.Resolve<IFeature>());

        private It should_resolve_as_unreleased = () => _resolvedFeature.ShouldBeOfType<UnreleasedFeature>();
        private It should_not_throw = () => _exception.ShouldBeNull();

        private static IFeature _resolvedFeature;
        private static IWindsorContainer _container;
        private static Exception _exception;
    }


    internal class UnreleasedFeature : IFeature
    {
    }

    internal class ReleasedFeature : IFeature
    {
        
    }

    internal interface IFeature
    {
    }
}
