using System;

namespace features
{
    public interface IFeatureSpec
    {
        Type FeatureType { get; }
        Type ImplementationType();
    }
}