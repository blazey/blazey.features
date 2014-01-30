using System;
using NUnit.Framework;

namespace blazey.features.specs
{
	[TestFixture]
	public abstract class context_specification
	{
		[SetUp]
		public void Setup()
		{
			Given();
			Exception = Catch.Exception(()=> When());
		}

		public abstract void Given();

		public abstract void When();  

		protected Exception Exception {
			get;
			private set;
		}      
	}


}

