using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace blazey.features.configuration
{
    public class FeaturesConfiguration
    {
        private readonly IList<KeyValuePair<Type, Type>> _services = new List<KeyValuePair<Type, Type>>();

        public void AddFeatueSpecification<TFeatureSpecifaction, TFeature>()
            where TFeatureSpecifaction : IFeatureSpecification<TFeature>
            where TFeature : class
        {
            var featureType = typeof (TFeature);
            var featureSpecificationType = typeof (IFeatureSpecification<>);

            if (featureType == featureSpecificationType)
            {
                throw new InvalidOperationException("Cannot nest feature specications");
            }
            
            _services.Add(new KeyValuePair<Type, Type>(typeof(TFeatureSpecifaction), typeof (TFeatureSpecifaction)));
        }

        public void ConfigureWindsor(IWindsorContainer container)
        {

            container.AddFacility<FeaturesFacility>();

            foreach (var service in _services)
            {
                container.Register(Component.For(service.Key).ImplementedBy(service.Value));
            }

        }

    }
}