using System;

namespace blazey.features
{
    internal class FeatureSpecificationType
    {
        internal static FeatureSpecificationType FromDependency(Type dependency)
        {

            return new FeatureSpecificationType(dependency);
        }

        internal Type FeatureSpecifactionType { get; private set; }

        internal Type Feature { get; private set; }

        public static Type OpenGenericType { get { return typeof (IFeatureSpecification<>); } }

        public static bool IsFeatureSpecification(Type candidate)
        {
            return candidate == OpenGenericType;
        }

        protected FeatureSpecificationType(Type targetItemType)
        {
            
            if(targetItemType == OpenGenericType) throw new ArgumentException("targetItemType is not IFeatureSpecification<T>");
            
            FeatureSpecifactionType = OpenGenericType.MakeGenericType(targetItemType);

            Feature = targetItemType.GetGenericArguments()[0];
        }
    }
}