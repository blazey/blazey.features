using Machine.Specifications;

namespace blazey.features.specs
{
    [Subject(typeof (when_machine_specs_compiles),
        "If mspec runner is working and test compiles, this test will always pass.")]
    internal class when_machine_specs_compiles
    {
        private It should_compile = () => true.ShouldBeTrue();
    }
}