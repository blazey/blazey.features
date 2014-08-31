using System;

namespace blazey.features
{
    public interface IFeatureMap
    {
        Type FeatureType { get; }
        Type ImplementationType();
    }
}