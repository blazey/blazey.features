using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel;

namespace blazey.features
{
    internal class FeatureHandlerSelector : IHandlerSelector
    {
        private readonly IKernel _kernel;
        private IDictionary<Type, Type> _featureSpecs = new ConcurrentDictionary<Type, Type>();

        public FeatureHandlerSelector(IKernel kernel)
        {
            _kernel = kernel;
        }

        public void AddFeatureSpecConfig(FeatureConfiguration featureConfiguration)
        {
            _featureSpecs = featureConfiguration.ConfigMap;
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

            if (null == featureSpec) return null;

            var handle = handlers.SingleOrDefault(h =>
                h.ComponentModel.Services.Any(s => s == featureSpec.FeatureType)
                && h.ComponentModel.Implementation == featureSpec.ImplementationType());

            return handle;
        }
    }
}