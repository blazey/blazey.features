using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using blazey.features.specs.doubles;
using NUnit.Framework;

namespace blazey.features.specs
{
	public class when_feature_implementation_is_on : context_specification
    {
		private bool _isOn;
		private WindsorContainer _windsorContainer;

		public override void Given ()
		{
			_windsorContainer = new WindsorContainer();

			_windsorContainer.AddFacility(FeaturesFacility.RegisterFeatureSpecifications(_windsorContainer, register =>
				register.AddFeatueSpecification<DummyFeatureSpecification, ISomeFeature>()));

			_windsorContainer.Register(Component.For<ISomeFeature>().ImplementedBy<ReleasedFeature>(),
				Component.For<FeatureTableService>());
		}

		public override void When ()
		{
			_isOn = _windsorContainer.Resolve<FeatureTableService>().IsOn<ISomeFeature, UnreleasedFeature>();
		}

		[Then]
		public void should_not_throw(){
			Assert.That(base.Exception, Is.Null);
		}

		[Then]
		public void should_resolve_as_released ()
		{
			Assert.That(_isOn, Is.True);
		}        
    }
}

