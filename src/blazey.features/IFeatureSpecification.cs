namespace blazey.features
{
    public interface IFeatureSpecification<out TFeature> : IFeatureOn where TFeature : class
    {
        TFeature Feature();
    }
}