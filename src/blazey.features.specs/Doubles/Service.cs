namespace blazey.features.specs.Doubles
{
    public class Service
    {
        public ISomeFeature Feature { get; private set; }

        public Service(ISomeFeature feature)
        {
            Feature = feature;
        }
    }
}