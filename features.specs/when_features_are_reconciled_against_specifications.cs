using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using blazey.features.specs.doubles;

namespace blazey.features.specs
{
	internal class when_features_are_reconciled_against_specifications : context_specification
	{
		private FeatureTableService _featureTableService;

		public override void Given ()
		{
			var container = new WindsorContainer ();

			container.AddFacility (
				FeaturesFacility.RegisterFeatureSpecifications (
					container, register => {
					register.AddFeatueSpecification<FeatureSpecificationX, FeatureX> ();
					register.AddFeatueSpecification<FeatureSpecificationY, FeatureY> ();
					register.AddFeatueSpecification<FeatureSpecificationZ, FeatureZ> ();
				}));

			container.Register (Component.For<FeatureZ> ().ImplementedBy<FeatureZ> (),
				Component.For<FeatureTableService> ());

			_featureTableService = container.Resolve<FeatureTableService> ();
		}

		public override void When ()
		{
			_featureTableService.ValidateSpecificiedFeaturesAreRegistered ();
		}

		[Then]
		public void should_throw_un_registered_feature_is_specificied_exception ()
		{
			Assert.That (base.Exception, Is.TypeOf<UnRegisteredFeatureIsSpecifiedException> ());
		}

		[Then]
		public void should_list_unregistered_features_containing_specification_x_and_feature_x ()
		{
			ResolvesInvalidSpecification<FeatureSpecificationX, FeatureX> (base.Exception);
		}

		[Then]
		public void should_list_unregistered_features_containing_specification_y_and_feature_ ()
		{
			ResolvesInvalidSpecification<FeatureSpecificationY, FeatureY> (base.Exception);
		}

		[Then]
		public void should_not_list_unregistered_features_containing_specification_z_and_feature_z ()
		{
			Assert.That (((UnRegisteredFeatureIsSpecifiedException)base.Exception)
				.InvalidSpecifications.ContainsKey (typeof(FeatureSpecificationZ)), Is.False);
		}

		private static void ResolvesInvalidSpecification<TFeatureSpecification, TFeature> (Exception exception)
		{
			var unRegisteredFeatureIsSpecifiedException = (UnRegisteredFeatureIsSpecifiedException)exception;

			var type = unRegisteredFeatureIsSpecifiedException
						.InvalidSpecifications [typeof(TFeatureSpecification)];

			Assert.That (type, Is.EqualTo (typeof(TFeature))); 

		}
	}
}