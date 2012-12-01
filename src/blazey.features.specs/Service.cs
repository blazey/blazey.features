namespace blazey.features.specs
{
    public class Service
    {
        public IFeature Feature { get; private set; }

        public Service(IFeature feature)
        {
            Feature = feature;
        }
    }
}