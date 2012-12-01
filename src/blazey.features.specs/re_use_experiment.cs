using Machine.Specifications;

namespace blazey.features.specs
{
    internal class re_use_experiment
    {

        public static int Number { get; private set; }

        private Establish establish = () =>
            {
                Number = 10;
            };

        private It should_be_10 = () => Number.ShouldEqual(10);
    }
}