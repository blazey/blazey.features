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

            foreach (var featureSpecType in featureConfiguration.ConfigMap)
            {
                var type = featureSpecType;
                _container.Kernel.Register(Component.For(featureSpecType.Value).ImplementedBy(featureSpecType.Value));
                _container.Kernel.Register(Component.For(featureSpecType.Key).UsingFactoryMethod(k =>
                {
                    var spec = (IFeatureMap)k.Resolve(type.Value);

                    k.ReleaseComponent(spec);

                    return spec.ImplementationType();

                }));
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