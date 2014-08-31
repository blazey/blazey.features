using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace features
{
    public class Specs
    {
        private readonly IWindsorContainer _container;

        public static Specs Features(IWindsorContainer container, Action<FeatureSpecConfig> featureSpecConfig)
        {
            return new Specs(container).WithFeatures(featureSpecConfig);
        }

        public Specs(IWindsorContainer container)
        {
            _container = container;
            _container.Kernel.AddHandlerSelector(new FeatureHandlerSelector(_container.Kernel));
        }

        private Specs(IWindsorContainer container, FeatureSpecConfig featureSpecConfig)
        {
            _container = container;
            var featureHandlerSelector = new FeatureHandlerSelector(_container.Kernel);
            featureHandlerSelector.AddFeatureSpecConfig(featureSpecConfig);

            _container.Kernel.AddHandlerSelector(featureHandlerSelector);

            foreach (var featureSpecType in featureSpecConfig.FeatureSpecs)
            {
                var type = featureSpecType;
                _container.Kernel.Register(Component.For(featureSpecType.Value).ImplementedBy(featureSpecType.Value));
                _container.Kernel.Register(Component.For(featureSpecType.Key).UsingFactoryMethod(k =>
                {
                    var spec = (IFeatureSpec)k.Resolve(type.Value);

                    k.ReleaseComponent(spec);

                    return spec.ImplementationType();

                }));
            }
        }

        public Specs WithFeatures(Action<FeatureSpecConfig> featureSpecConfig)
        {
            var config = new FeatureSpecConfig();
            featureSpecConfig(config);
            return new Specs(_container, config);
        }
    }
}