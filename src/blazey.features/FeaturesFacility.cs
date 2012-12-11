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

        public void Terminate()
        {
        }

        public static void AddFeatueSpecification<TFeatureSpecifaction, TFeature>(IWindsorContainer container, Action<FeaturesConfiguration> config) 
            where TFeatureSpecifaction : IFeatureSpecification<TFeature> where TFeature : class
        {

            if (null == container) throw new ArgumentNullException("container");

            var configInstance = new FeaturesConfiguration();
            configInstance.AddFeatueSpecification<TFeatureSpecifaction, TFeature>();
            configInstance.ConfigureWindsor(container);
        }
    }
}