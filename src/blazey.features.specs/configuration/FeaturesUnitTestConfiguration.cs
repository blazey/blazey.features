using Castle.MicroKernel.Registration;
using Castle.Windsor;
using blazey.features.configuration;

namespace blazey.features.specs.configuration
{
    internal class FeaturesUnitTestConfiguration : FeaturesConfiguration
    {
        public FeaturesUnitTestConfiguration()
        {
            Container = new WindsorContainer();
        }

        public IWindsorContainer Container { get; private set; }

        internal void RegisterComponent(params IRegistration[] registration)
        {
            Container.Register(registration);
        }
    }
}