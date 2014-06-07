namespace blazey.features.specs.doubles
{
    internal class FeatureSpecificationY : IFeatureSpecification<FeatureY>
    {
        public FeatureY Feature()
        {
            return new FeatureY();
        }

        public bool On()
        {
            return true;
        }
    }
}