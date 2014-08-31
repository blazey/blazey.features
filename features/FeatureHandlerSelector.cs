using System;
using System.Collections.Concurrent;
using System.Linq;
using Castle.MicroKernel;

namespace blazey.features
{
    internal class FeatureHandlerSelector : IHandlerSelector
    {
        private readonly IKernel _kernel;
        private readonly ConcurrentDictionary<Type, Type> _featureSpecs = new ConcurrentDictionary<Type, Type>();

        public FeatureHandlerSelector(IKernel kernel)
        {
            _kernel = kernel;
        }

        public void AddFeatureSpecConfig(FeatureConfiguration featureConfiguration)
        {
            foreach (var featureSpecType in featureConfiguration.ConfigMap)
            {
                _featureSpecs.AddOrUpdate(featureSpecType.Key, featureSpecType.Value,
                    (k, v) => featureSpecType.Value);
            }
        }

        public bool HasOpinionAbout(string key, Type service)
        {
            return _featureSpecs.ContainsKey(service);
        }

        public IHandler SelectHandler(string key, Type service, IHandler[] handlers)
        {
            Type spec;
            _featureSpecs.TryGetValue(service, out spec);

            if (null == spec) return null;
         
            var featureSpec = _kernel.Resolve(spec) as IFeatureMap;

            if(null == featureSpec) return null;

            var handle = handlers.SingleOrDefault(h =>
                h.ComponentModel.Services.Any(s => s == featureSpec.FeatureType)
                && h.ComponentModel.Implementation == featureSpec.ImplementationType());

            return handle;


        }
    }
}