namespace blazey.features.specs.Doubles
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