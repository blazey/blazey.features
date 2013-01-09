using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Castle.Core;
using Castle.MicroKernel;

namespace blazey.features
{
    [Serializable]
    public class UnRegisteredFeatureIsSpecifiedException : Exception
    {
        protected UnRegisteredFeatureIsSpecifiedException(IDictionary<Type, Type> invalidSpecifications)
        {
            InvalidSpecifications = invalidSpecifications;
        }

        protected UnRegisteredFeatureIsSpecifiedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public IDictionary<Type, Type> InvalidSpecifications { get; private set; }

        public static void ThrowIfUnregistered(IKernel kernel)
        {
            var unregistered = kernel
                .GraphNodes.OfType<ComponentModel>()
                .Where(component => FeatureSpecificationType.IsFeatureSpecification(component.Implementation))
                .ToDictionary(key => key.Implementation,
                              value => FeatureSpecificationType.ToFeature(value.Implementation))
                .Where(kvp => !kernel.HasComponent(kvp.Value))
                .ToDictionary(key => key.Key, value => value.Value);

            if (unregistered.Any()) throw new UnRegisteredFeatureIsSpecifiedException(unregistered);
        }
    }
}