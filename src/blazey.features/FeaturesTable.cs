using Castle.MicroKernel;

namespace blazey.features
{
    public class FeaturesTable
    {
        private readonly IKernel _kernel;

        public FeaturesTable(IKernel kernel)
        {
            _kernel = kernel;
        }

        public bool IsOn<TService, TFeature>()
        {
            
            var featureSpecificationType = FeatureSpecificationType.FromFeature(typeof (TService));
            var featureSpecification = _kernel.Resolve(featureSpecificationType) as IFeatureSpecification<object>;
            
            return null != featureSpecification 
                && featureSpecification.Feature().GetType() == typeof (TFeature);
        }
    }
}