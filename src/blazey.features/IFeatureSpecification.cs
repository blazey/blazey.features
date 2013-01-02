namespace blazey.features
{
    public interface IFeatureOn
    {
        bool On();
    }

    public interface IFeatureSpecification<out TFeature> : IFeatureOn where TFeature : class
    {
        TFeature Feature();
    }
}