namespace blazey.features.specs.doubles
{
    internal class FeatureSpecificationZ : IFeatureSpecification<FeatureZ>
    {
        private readonly FeatureZ _featureZ;

        public FeatureSpecificationZ(FeatureZ featureZ)
        {
            _featureZ = featureZ;
        }

        public FeatureZ Feature()
        {
            return _featureZ;
        }

        public bool On()
        {
            return true;
        }
    }
}