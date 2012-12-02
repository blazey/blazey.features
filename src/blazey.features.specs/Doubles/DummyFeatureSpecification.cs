using blazey.features.specs.Doubles;

namespace blazey.features.specs
{
    internal class DummyFeatureSpecification : IFeatureSpecification<IFeature>
    {
        public bool Default()
        {
            return false;
        }

        public IFeature Feature()
        {
            return new UnreleasedFeature();
        }
    }
}