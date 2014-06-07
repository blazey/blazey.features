using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using blazey.features.specs.doubles;
using NUnit.Framework;

namespace blazey.features.specs
{
	internal class when_feature_is_unreleased : context_specification
    {

		public override void Given ()
		{
                _windsorContainer = new WindsorContainer();
                _windsorContainer.AddFacility(FeaturesFacility.RegisterFeatureSpecifications(
                    _windsorContainer, register =>
                                       register.AddFeatueSpecification<DummyFeatureSpecification, ISomeFeature>()));

                _windsorContainer.Register(Component.For<ISomeFeature>().ImplementedBy<ReleasedFeature>(),
                                           Component.For<ServiceWithAFeature>());
            }

		public override void When ()
		{
			_resolvedFeature = _windsorContainer.Resolve<ServiceWithAFeature> ().Feature;
		}

		[Then]
		public void should_not_throw(){
			Assert.That(base.Exception, Is.Null);
		}

		[Then]
		public void should_resolve_as_unreleased(){
			Assert.That (_resolvedFeature, Is.TypeOf<UnreleasedFeature> ());
		}

        private object _resolvedFeature;
        private WindsorContainer _windsorContainer;

    }
}