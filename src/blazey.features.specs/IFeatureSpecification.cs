namespace blazey.features.specs
{
    internal interface IFeatureSpecification<out TFeature> where TFeature : class
    {
        bool Default();
        TFeature Feature();
    }
}