using System;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Machine.Specifications;
using blazey.features.specs.Doubles;

namespace blazey.features.specs
{
    [Subject(typeof(FeatureResolver))]
    public class when_facility_tries_to_resolve_non_specifcation_facility
    {
        private Establish that = () =>
            {
                var target = typeof (DummyFeatureSpecification);
                _dependencyModel = new DependencyModel(target.FullName, target, false);
                var container = new WindsorContainer();
                container.Register(
                    Component.For<Service>(),
                    Component.For<IFeatureSpecification<ISomeFeature>>().ImplementedBy<DummyFeatureSpecification>());

                _resolver = new FeatureResolver(container.Kernel);
            };

        private Because can_not_resolve_object = () => _exception = Catch.Exception(
            ()=> _canResolve = _resolver.CanResolve(null, null, null, _dependencyModel));

        private It should_not_be_able_to_resolve = () => _canResolve.ShouldBeFalse();

        private It should_not_throw = () => _exception.ShouldBeNull();

        private static bool _canResolve;
        private static Exception _exception;
        private static FeatureResolver _resolver;
        private static DependencyModel _dependencyModel;
    }
}