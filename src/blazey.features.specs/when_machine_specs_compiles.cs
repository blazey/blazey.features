using Machine.Specifications;

namespace blazey.features.specs
{
    [Subject(typeof (when_machine_specs_compiles),
        "If mspec runner is working and test compiles, this test will always pass.")]
    internal class when_machine_specs_compiles
    {
        private re_use_experiment re_use = new re_use_experiment();

        private It should_compile = () => true.ShouldBeTrue();
    }
}