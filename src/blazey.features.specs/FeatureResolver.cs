using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;

namespace blazey.features.specs
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
            return true;
        }

        public object Resolve(CreationContext context, ISubDependencyResolver contextHandlerResolver,
                              ComponentModel model, DependencyModel dependency)
        {

            var featurespecificationType = typeof (IFeatureSpecification<>)
                .GetGenericTypeDefinition()
                .MakeGenericType(new[] {dependency.TargetItemType});

            var featureSpecification = (IFeatureSpecification<object>) _kernel.Resolve(featurespecificationType);

            return featureSpecification.Default() ? _kernel.Resolve(dependency.TargetItemType) : featureSpecification.Feature();
        }
    }
}