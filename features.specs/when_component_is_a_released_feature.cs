using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using blazey.features.specs.doubles;
using NUnit.Framework;

namespace blazey.features.specs
{
	public class when_component_is_a_released_feature : context_specification
    {
        private ISomeFeature _resolvedFeature;
        private IWindsorContainer _container;

		public override void Given(){
			_container = new WindsorContainer();
			_container.Register(Component.For<ISomeFeature>().ImplementedBy<ReleasedFeature>());
		}

		public override void When(){
			_resolvedFeature = _container.Resolve<ISomeFeature> ();
		}

		[Then]
		public void should_not_throw(){
			Assert.That(base.Exception, Is.Null);
		}

		[Then]
		public void should_resolve_as_released(){
			Assert.That (_resolvedFeature, Is.TypeOf<ReleasedFeature> ());
		}
    }
}