using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;

namespace blazey.features
{
    public class FeatureResolver : ISubDependencyResolver
    {
        private readonly IKernel _kernel;

        public FeatureResolver(IKernel kernel)
        {
            _kernel = kernel;
        }

        public bool CanResolve(CreationContext context, ISubDependencyResolver contextHandlerResolver,
                               ComponentModel model, DependencyModel dependency)
        {
            return FeatureSpecificationType.KernelCanResolve(_kernel, dependency);
        }

        public object Resolve(CreationContext context, ISubDependencyResolver contextHandlerResolver,
                              ComponentModel model, DependencyModel dependency)
        {
 
            var featurespecificationType = FeatureSpecificationType.FromFeature(dependency.TargetItemType);

            var featureSpecification = (IFeatureSpecification<object>) _kernel.Resolve(featurespecificationType);

            return featureSpecification.On()
                       ? _kernel.Resolve(dependency.TargetItemType)
                       : featureSpecification.Feature();
        }
    }
}