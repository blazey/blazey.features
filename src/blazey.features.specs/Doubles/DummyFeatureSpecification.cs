namespace blazey.features.specs.Doubles
{
    internal class DummyFeatureSpecification : IFeatureSpecification<ISomeFeature>
    {
        public bool On()
        {
            return false;
        }

        public ISomeFeature Feature()
        {
            return new UnreleasedFeature();
        }
    }
}