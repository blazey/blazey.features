using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace blazey.features
{
    public class Features
    {
        private readonly IWindsorContainer _container;

        public static Features Configure(IWindsorContainer container, Action<FeatureConfiguration> featureSpecConfig)
        {
            return new Features(container).WithFeatures(featureSpecConfig);
        }

        public Features(IWindsorContainer container)
        {
            _container = container;
            _container.Kernel.AddHandlerSelector(new FeatureHandlerSelector(_container.Kernel));
        }

        private Features(IWindsorContainer container, FeatureConfiguration featureConfiguration)
        {
            _container = container;
            var featureHandlerSelector = new FeatureHandlerSelector(_container.Kernel);
            featureHandlerSelector.AddFeatureSpecConfig(featureConfiguration);

            _container.Kernel.AddHandlerSelector(featureHandlerSelector);

            foreach (var featureSpecType in featureConfiguration.ConfigMap
                /* TODO: .Where(kvp => typeof(IFeatureMap).IsAssignableFrom(kvp.Value))*/)
            {
                _container.Kernel.Register(Component.For(featureSpecType.Value).ImplementedBy(featureSpecType.Value));
            }
        }

        public Features WithFeatures(Action<FeatureConfiguration> featureSpecConfig)
        {
            var config = new FeatureConfiguration();
            featureSpecConfig(config);
            return new Features(_container, config);
        }
    }
}