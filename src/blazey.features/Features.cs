using Castle.MicroKernel;

namespace blazey.features
{
    public class Features
    {
        private readonly IKernel _kernel;

        public Features(IKernel kernel)
        {
            _kernel = kernel;
        }

        public bool IsOn<TService, TFeature>()
        {
            
            var serviceType = typeof (TService);
            var featureSpecificationType = FeatureSpecificationType.FromFeature(serviceType);
            var featureSpecification = _kernel.Resolve(featureSpecificationType) as IFeatureSpecification<object>;
            
            return null != featureSpecification 
                && featureSpecification.Feature().GetType() == typeof (TFeature);
        }
    }
}