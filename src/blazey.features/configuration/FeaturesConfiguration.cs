using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace blazey.features.configuration
{
    public class FeaturesConfiguration
    {
        readonly List<Tuple<Type, Type>> _services = new List<Tuple<Type, Type>>();

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
            
            _services.Add(new Tuple<Type, Type>(featureType, typeof (TFeatureSpecifaction)));
        }

        public void AddFeature<TService, TImplmentation>()
        {
            _services.Add(new Tuple<Type, Type>(typeof(TService), typeof(TImplmentation))); 
        }

        public void ConfigureWindsor(IWindsorContainer container)
        {

            container.AddFacility<FeaturesFacility>();

            foreach (var service in _services)
            {

                var featureType = FeatureSpecificationType.FromType(service.Item2).Feature;

                if (!container.Kernel.HasComponent(featureType))
                {
                    throw new InvalidOperationException(
                        "Cannot register feature specifaction component because the feature is not registered.");
                }

                container.Register(Component.For(service.Item1).ImplementedBy(service.Item2));
            }

        }

    }
}