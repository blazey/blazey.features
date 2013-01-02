namespace blazey.features.specs.doubles
{
    internal class FeatureTableService
    {
        private readonly FeaturesTable _featureTableTable;

        public FeatureTableService(Features features)
        {
            _featureTableTable = features();
        }

        public bool IsOn<TService, TFeature>()
        {
            return _featureTableTable.IsOn<TService, TFeature>();
        }

    }
}