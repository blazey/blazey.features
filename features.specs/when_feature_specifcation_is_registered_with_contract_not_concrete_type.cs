using System;
using NUnit.Framework;
using blazey.features.configuration;
using blazey.features.specs.doubles;

namespace blazey.features.specs
{
	public class when_feature_specifcation_is_registered_with_contract_not_concrete_type : context_specification
	{
		FeaturesConfiguration _featuresConfiguration;

		public override void Given ()
		{
			_featuresConfiguration = new FeaturesConfiguration();
		}

		public override void When ()
		{
			_featuresConfiguration.AddFeatueSpecification<IFeatureSpecification<ISomeFeature>, ISomeFeature> ();
		}

		[Then]
		public void should_throw_invalid_operation() {
			Assert.That (Exception, Is.TypeOf<InvalidOperationException> ());
		}
	}
}