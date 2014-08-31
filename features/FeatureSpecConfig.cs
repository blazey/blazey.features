using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace features
{
    public class FeatureSpecConfig
    {
        public IDictionary<Type,Type> FeatureSpecs { get; private set; }

        public FeatureSpecConfig()
        {
            FeatureSpecs=new ConcurrentDictionary<Type, Type>();
        }

        private FeatureSpecConfig(IEnumerable<KeyValuePair<Type, Type>> featureSpecs)
        {
            FeatureSpecs = new ConcurrentDictionary<Type, Type>(featureSpecs);
        }

        public FeatureSpecConfig Add<TFeature, TFeatureSpec>() where TFeatureSpec : IFeatureSpec
        {
            FeatureSpecs.Add(typeof (TFeature), typeof(TFeatureSpec));
            return new FeatureSpecConfig(FeatureSpecs);
        }
    }
}