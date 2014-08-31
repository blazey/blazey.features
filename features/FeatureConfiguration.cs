using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace blazey.features
{
    public class FeatureConfiguration
    {
        public IDictionary<Type,Type> ConfigMap { get; private set; }

        public FeatureConfiguration()
        {
            ConfigMap = new ConcurrentDictionary<Type, Type>();
        }

        private FeatureConfiguration(IEnumerable<KeyValuePair<Type, Type>> config)
        {
            ConfigMap = new ConcurrentDictionary<Type, Type>(config);
        }

        public FeatureConfiguration UseFeatureMap<TFeature, TFeatureMap>() where TFeatureMap : IFeatureMap
        {
            ConfigMap.Add(typeof (TFeature), typeof(TFeatureMap));
            return new FeatureConfiguration(ConfigMap);
        }
    }
}