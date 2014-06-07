using System;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using blazey.features.specs.doubles;

namespace blazey.features.specs
{
	public class when_feature_tries_to_resolve_none_specification_facility: context_specification
	{

		private FeatureResolver _resolver;
		private bool _canResolve;

		private DependencyModel _dependencyModel;

		public override void Given ()
		{
			var target = typeof (DummyFeatureSpecification);
			_dependencyModel = new DependencyModel(target.FullName, target, false);
			var container = new WindsorContainer();
			container.Register(
				Component.For<ServiceWithAFeature>(),
				Component.For<IFeatureSpecification<ISomeFeature>>().ImplementedBy<DummyFeatureSpecification>());

			_resolver = new FeatureResolver(container.Kernel);
		}

		public override void When ()
		{
			_canResolve = _resolver.CanResolve (null, null, null, _dependencyModel);
		}

		[Then]
		public void should_not_throw() {
			Assert.That (base.Exception, Is.Null);
		}

		[Then]
		public void should_not_be_able_to_resolve(){
			Assert.That (_canResolve, Is.False);
		}
		 
    }
}