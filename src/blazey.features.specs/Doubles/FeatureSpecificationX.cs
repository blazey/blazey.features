namespace blazey.features.specs.doubles
{
    internal class FeatureSpecificationX : IFeatureSpecification<FeatureX>
    {
        public FeatureX Feature()
        {
            return new FeatureX();
        }

        public bool On()
        {
            return true;
        }
    }
}