using System;
using Castle.MicroKernel;

namespace blazey.features
{
    public class FeatureTableImpl
    {
        private readonly IKernel _kernel;

        public FeatureTableImpl(IKernel kernel)
        {
            _kernel = kernel;
        }

        public bool IsOn<TFeature>()
        {
            var featureType = typeof (TFeature);
            var featureSpecificationType = FeatureSpecificationType.FromFeature(featureType);
            var featureSpecification = _kernel.Resolve(featureSpecificationType) as IFeatureSpecification<object>;
            
            if (null == featureSpecification) return false;

            return featureSpecification.Feature().GetType() == typeof (TFeature);

        }
    }
}