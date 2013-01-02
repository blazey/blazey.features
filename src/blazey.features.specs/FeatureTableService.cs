namespace blazey.features.specs
{
    internal class FeatureTableService
    {
        private readonly Features _featureTable;

        public FeatureTableService(FeatureTable featureTable)
        {
            _featureTable = featureTable();
        }

        public bool IsOn<TService, TFeature>()
        {
            return _featureTable.IsOn<TService, TFeature>();
        }

    }
}