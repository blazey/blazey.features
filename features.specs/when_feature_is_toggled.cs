using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using blazey.features.specs;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;

namespace features.specs
{
    internal class when_feature_is_toggled : context_specification
    {
        private IFeature _feature;
        private IWindsorContainer _container;

        public override void Given()
        {
            _container = new WindsorContainer()
                .Register(
                    Component.For<FeatureA>().ImplementedBy<FeatureA>(),
                    Component.For<FeatureB>().ImplementedBy<FeatureB>());

            Specs.Features(_container, c => c.Add<IFeature, FeatureSpecAtoB>());

        }

        public override void When()
        {
            _feature = _container.Resolve<IFeature>();
        }

        [Test]
        public void should_be_feature_b()
        {
            Assert.That(_feature, Is.InstanceOf<FeatureB>());
        }

        [Test]
        public void should_not_be_feature_a()
        {
            Assert.That(_feature, Is.Not.InstanceOf<FeatureA>());
        }

        [Test]
        public void should_not_be_null()
        {
            Assert.That(_feature, Is.Not.Null);
        }

        [Test]
        public void should_not_throw()
        {
            Assert.That(Exception, Is.Null);
        }


        internal interface IFeature
        {
        }

        private class FeatureA : IFeature
        {

        }

        private class FeatureB : IFeature
        {
        }

        private class FeatureSpecAtoB : IFeatureSpec
        {
            public Type FeatureType { get; private set; }

            public Type ImplementationType()
            {
                return typeof (FeatureB);
            }
        }
    }

    internal class FeatureHandlerSelector : IHandlerSelector
    {
        private readonly IKernel _kernel;
        private readonly ConcurrentDictionary<Type, Type> _featureSpecs = new ConcurrentDictionary<Type, Type>();

        public FeatureHandlerSelector(IKernel kernel)
        {
            _kernel = kernel;
        }

        public void AddFeatureSpecConfig(FeatureSpecConfig featureSpecConfig)
        {
            foreach (var featureSpecType in featureSpecConfig.FeatureSpecs)
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
         
            var featureSpec = _kernel.Resolve(spec) as IFeatureSpec;

            if(null == featureSpec) return null;

            var handle= handlers.SingleOrDefault(h => h.ComponentModel.Services.Any(s => s ==  featureSpec.ImplementationType()));

            return handle;


        }
    }

    internal class Specs
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

    internal class FeatureSpecConfig
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

        internal FeatureSpecConfig Add<TFeature, TFeatureSpec>() where TFeatureSpec : IFeatureSpec
        {
            FeatureSpecs.Add(typeof (TFeature), typeof(TFeatureSpec));
            return new FeatureSpecConfig(FeatureSpecs);
        }
    }

    interface IFeatureSpec
    {
        Type FeatureType { get; }
        Type ImplementationType();
    }


}
