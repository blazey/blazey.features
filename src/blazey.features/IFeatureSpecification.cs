namespace blazey.features
{
    public interface IFeatureSpecification<out TFeature> where TFeature : class
    {
        bool Default();
        TFeature Feature();
    }
}