using System;
using Castle.Windsor;
using Machine.Specifications;
using blazey.features.configuration;

namespace blazey.features.specs
{
    public class Features
    {
        public static IWindsorContainer Container { get; private set; }

        public Features()
        {
            Container = new WindsorContainer();
            Container.AddFacility<FeaturesFacility>();
        }

        internal Establish ConfigureWindsor(Action<FeaturesConfiguration> config)
        {

            var c = new FeaturesConfiguration();
            c.ConfigureWindsor(Container);
            config(c);

            return () => { };
/*
            return () =>
                {
                    Container = new WindsorContainer();
                    Container.Kernel.Resolver.AddSubResolver(new FeatureResolver(Container.Kernel));
                    Container.Register(
                        Component.For<Service>(),
                        Component.For<ISomeFeature>().ImplementedBy<ReleasedFeature>(),
                        Component.For<IFeatureSpecification<ISomeFeature>>().ImplementedBy<DummyFeatureSpecification>());
                };
*/
        }

        internal T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

    }
}