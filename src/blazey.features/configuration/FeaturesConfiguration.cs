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
            var implementation = typeof (TFeatureSpecifactionImplementation);

            if (implementation.IsAbstract || implementation.IsInterface)
            {
                throw new InvalidOperationException("must be a concrete type");
            }

            var featureType = typeof (TFeature);

            if (FeatureSpecificationType.IsFeatureSpecification(featureType))
            {
                throw new InvalidOperationException("Cannot nest feature specications");
            }

            var service = FeatureSpecificationType.FromFeature(featureType).FeatureSpecifactionType;
            
            _services.Add(new KeyValuePair<Type, Type>(service, implementation));
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