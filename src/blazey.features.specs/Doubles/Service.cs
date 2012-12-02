namespace blazey.features.specs.Doubles
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