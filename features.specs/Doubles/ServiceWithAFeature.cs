namespace blazey.features.specs.doubles
{
    public class ServiceWithAFeature
    {
        public object Feature { get; private set; }

        public ServiceWithAFeature(ISomeFeature feature)
        {
            Feature = feature;
        }
    }
}