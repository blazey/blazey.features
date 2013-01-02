using System;
using Castle.Core.Configuration;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using blazey.features.configuration;

namespace blazey.features
{
    public class FeaturesFacility : IFacility
    {

        public void Init(IKernel kernel, IConfiguration facilityConfig)
        {
            kernel.Resolver.AddSubResolver(new FeatureResolver(kernel));
            kernel.Register(Component.For<Features>().Instance(() => new FeaturesTable(kernel)));
        }

        public static FeaturesFacility RegisterFeatureSpecifications(IWindsorContainer container, Action<FeaturesConfiguration> register) 
        {

            if (null == container) throw new ArgumentNullException("container");
           
            return new FeaturesFacility(container, register);
        }

        public void Terminate()
        {
        }

        private FeaturesFacility(IWindsorContainer container, Action<FeaturesConfiguration> register)
        {
            var configuration = new FeaturesConfiguration();
            register(configuration);
            configuration.ConfigureWindsor(container);
        }


    }
}