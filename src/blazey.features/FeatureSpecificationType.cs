using System;

namespace blazey.features
{
    internal class FeatureSpecificationType
    {

        internal static FeatureSpecificationType FromType(Type featureSpecifactionType)
        {
            return new FeatureSpecificationType(featureSpecifactionType);
        }
        internal static FeatureSpecificationType FromType<TFeatureSpecifactionType>() where TFeatureSpecifactionType : IFeatureSpecification<object>
        {
            return new FeatureSpecificationType(typeof(TFeatureSpecifactionType));
        }

        internal Type FeatureSpecifactionType { get; private set; }

        internal Type Feature { get; private set; }

        protected FeatureSpecificationType(Type targetItemType)
        {
            var featureSpecifcationType = typeof (IFeatureSpecification<>);

            if(targetItemType == featureSpecifcationType) throw new ArgumentException("targetItemType is not IFeatureSpecification<T>");
            
            FeatureSpecifactionType = featureSpecifcationType.MakeGenericType(targetItemType);

            Feature = targetItemType.GetGenericArguments()[0];
        }

    }
}