using blazey.features.specs.Doubles;

namespace blazey.features.specs
{
    internal class FeatureSpecification : IFeatureSpecification<IFeature>
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