using System;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Machine.Specifications;

namespace blazey.features.specs
{
    [Subject(typeof (when_machine_specs_compiles),
        "If mspec runner is working and test compiles, this test will always pass.")]
    internal class when_machine_specs_compiles
    {
        private re_use_experiment re_use = new re_use_experiment();

        private It should_compile = () => true.ShouldBeTrue();
    }

    internal class re_use_experiment
    {

        public static int Number { get; private set; }

        private Establish establish = () =>
            {
                Number = 10;
            };

        private It should_be_10 = () => Number.ShouldEqual(10);
    }

    internal class when_component_is_a_released_feature
    {
        private static IFeature _resolvedFeature;
        private static IWindsorContainer _container;
        private static Exception _exception;
        private It should_not_throw = () => _exception.ShouldBeNull();
        private It should_resolve_as_released = () => _resolvedFeature.ShouldBeOfType<ReleasedFeature>();

        private Establish that_windsor_is_configured = () =>
            {
                _container = new WindsorContainer();
                _container.Register(Component.For<IFeature>().ImplementedBy<ReleasedFeature>());
            };

        private Because windsor_resolves = () => _exception = Catch.Exception(
            () => _resolvedFeature = _container.Resolve<IFeature>());
    }

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

    public class Service
    {
        public IFeature Feature { get; private set; }

        public Service(IFeature feature)
        {
            Feature = feature;
        }
    }

    public class FeatureResolver : ISubDependencyResolver
    {
        private readonly IKernel _kernel;

        public FeatureResolver(IKernel kernel)
        {
            _kernel = kernel;

        }

        public bool CanResolve(CreationContext context, ISubDependencyResolver contextHandlerResolver,
                               ComponentModel model, DependencyModel dependency)
        {
            return true;
        }

        public object Resolve(CreationContext context, ISubDependencyResolver contextHandlerResolver,
                              ComponentModel model, DependencyModel dependency)
        {

            var featurespecificationType = typeof (IFeatureSpecification<>)
                .GetGenericTypeDefinition()
                .MakeGenericType(new[] {dependency.TargetItemType});

            var featureSpecification = (IFeatureSpecification<object>) _kernel.Resolve(featurespecificationType);

            return featureSpecification.Default() ? _kernel.Resolve(dependency.TargetItemType) : featureSpecification.Feature();
        }
    }

    internal interface IFeatureSpecification<out TFeature> where TFeature : class
    {
        bool Default();
        TFeature Feature();
    }

    internal class FeatureSpecification : IFeatureSpecification<IFeature>
    {
        public bool Default()
        {
            return false;
        }

        public IFeature Feature()
        {
            return new UnreleasedFeature();
        }
    }

    internal class UnreleasedFeature : IFeature
    {
    }

    internal class ReleasedFeature : IFeature
    {
    }

    public interface IFeature
    {
    }
}