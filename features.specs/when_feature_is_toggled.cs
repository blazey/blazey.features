using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;

namespace blazey.features.specs
{
    internal class when_feature_is_toggled : context_specification
    {
        private IFeature _feature;
        private IWindsorContainer _container;

        public override void Given()
        {
            _container = new WindsorContainer()
                .Register(
                    Component.For<IFeature>().ImplementedBy<FeatureA>(),
                    Component.For<IFeature>().ImplementedBy<FeatureB>());

            Features.Configure(_container, c => c.UseFeatureMap<IFeature, FeatureMapAtoB>());
        }

        public override void When()
        {
            _feature = _container.Resolve<IFeature>();
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

        private class FeatureMapAtoB : IFeatureMap
        {
            public Type FeatureType
            {
                get { return typeof (IFeature); }
            }

            public Type ImplementationType()
            {
                return typeof (FeatureB);
            }
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
    }
}