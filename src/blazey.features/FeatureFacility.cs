using System;
using Castle.Core.Configuration;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace blazey.features
{
    public class FeatureFacility : IFacility
    {
        public void Init(IKernel kernel, IConfiguration facilityConfig)
        {
            kernel.Resolver.AddSubResolver(new FeatureResolver(kernel));
        }

        public void Terminate()
        {
            throw new NotImplementedException();
        }

        public static void AddFeatueSpecification<TFeatureSpecifaction, TFeature>(IWindsorContainer container) 
            where TFeatureSpecifaction : IFeatureSpecification<TFeature> where TFeature : class
        {

            if (null == container) throw new ArgumentNullException("container");

            var featureType = typeof(TFeature);
            var featureSpecificationType = typeof(IFeatureSpecification<>);

            if (featureType == featureSpecificationType)
            {
                throw new InvalidOperationException("Cannot nest feature specications");
            }

            if (!container.Kernel.HasComponent(featureType))
            {
                throw new InvalidOperationException("Cannot register feature specifaction component because the feature is not registered.");
            }

            var contract = FeatureSpecificationType.Make(featureType);

            container.Register(Component.For(contract).ImplementedBy<TFeatureSpecifaction>());

        }
    }
}