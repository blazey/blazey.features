namespace blazey.features.specs.Doubles
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