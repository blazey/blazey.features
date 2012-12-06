using System;
using Castle.Core;
using Castle.MicroKernel;

namespace blazey.features
{
    internal class FeatureSpecificationType
    {
        internal static FeatureSpecificationType FromFeature(Type dependency)
        {

            return new FeatureSpecificationType(dependency);
        }

        internal static bool KernelCanResolve(IKernel kernel, DependencyModel dependency)
        {
            if (IsFeatureSpecification(dependency.TargetItemType)) return false;

            var featureSpecificationType = FromFeature(dependency.TargetItemType).FeatureSpecifactionType;

            return kernel.HasComponent(featureSpecificationType);

        }

        internal Type FeatureSpecifactionType { get; private set; }

        public static Type OpenGenericType { get { return typeof (IFeatureSpecification<>); } }

        public static bool IsFeatureSpecification(Type candidate)
        {
            var x =OpenGenericType.IsSubclassOf(candidate);
            var y = candidate.IsSubclassOf(OpenGenericType);
            return candidate == OpenGenericType;
        }

        protected FeatureSpecificationType(Type targetItemType)
        {
            
            if(targetItemType == OpenGenericType) throw new ArgumentException("targetItemType is not IFeatureSpecification<T>");
            
            FeatureSpecifactionType = OpenGenericType.MakeGenericType(targetItemType);

        }
    }
}