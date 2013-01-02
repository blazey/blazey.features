using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace blazey.features.configuration
{
    public class FeaturesConfiguration
    {
        private readonly IList<KeyValuePair<Type, Type>> _services = new List<KeyValuePair<Type, Type>>();

        public void AddFeatueSpecification<TFeatureSpecifactionImplementation, TFeature>()
            where TFeatureSpecifactionImplementation : IFeatureSpecification<TFeature>
            where TFeature : class
        {

            FeatureSpecificationType.ThrowIfNotConcrete<TFeatureSpecifactionImplementation>();

            var featureType = typeof (TFeature);
            var featureSpecifiationContract = FeatureSpecificationType.FromFeature(featureType);
            var implementation = typeof(TFeatureSpecifactionImplementation);

            _services.Add(new KeyValuePair<Type, Type>(featureSpecifiationContract, implementation));
        }

        public void ConfigureWindsor(IWindsorContainer container)
        {
            
            foreach (var service in _services)
            {
                container.Register(Component.For(service.Key).ImplementedBy(service.Value));
            }

        }

    }
}