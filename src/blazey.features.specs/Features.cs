using System;
using Castle.Windsor;
using Machine.Specifications;
using blazey.features.configuration;

namespace blazey.features.specs
{
    public class Features
    {
        public IWindsorContainer Container { get; private set; }

        internal Establish ConfigureWindsor(IWindsorContainer container, Action<FeaturesConfiguration> config)
        {
            if(null == container) throw new ArgumentNullException("container");
            Container = container;
            var featuresConfiguration = new FeaturesConfiguration();
            featuresConfiguration.ConfigureWindsor(Container);
            config(featuresConfiguration);

            return () => { };
        }

        internal T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

    }
}