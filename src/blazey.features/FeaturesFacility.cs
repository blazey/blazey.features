using System;
using Castle.Core.Configuration;
using Castle.MicroKernel;
using Castle.Windsor;
using blazey.features.configuration;

namespace blazey.features
{
    public class FeaturesFacility : IFacility
    {
        public void Init(IKernel kernel, IConfiguration facilityConfig)
        {
            kernel.Resolver.AddSubResolver(new FeatureResolver(kernel));
        }

        public static void ConfigureFeatureSpecifications(IWindsorContainer container, Action<FeaturesConfiguration> register) 
        {

            if (null == container) throw new ArgumentNullException("container");

            var configuration = new FeaturesConfiguration();
            register(configuration);
            configuration.ConfigureWindsor(container);
        }

        public void Terminate()
        {
        }

    }
}