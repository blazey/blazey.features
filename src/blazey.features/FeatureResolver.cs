using System;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;

namespace blazey.features
{
    public class FeatureResolver : ISubDependencyResolver
    {
        private readonly IKernel _kernel;

        public FeatureResolver(IKernel kernel)
        {
            _kernel = kernel;
            //TODO: if IFeatureSpecification exists and isn't configured? Throw, offer as own behavior so testable.

        }

        public bool CanResolve(CreationContext context, ISubDependencyResolver contextHandlerResolver,
                               ComponentModel model, DependencyModel dependency)
        {
            //TODO: is not null
            //TODO: is IFeatureSpecification<T>?
            //TODO: and T is registered by kernal already
            //TODO: identify if type has corresponding IFeatureSpecification<T>
            return true;
        }

        public object Resolve(CreationContext context, ISubDependencyResolver contextHandlerResolver,
                              ComponentModel model, DependencyModel dependency)
        {
            /*
             * TODO: If type has corrsponding IFeatureSpecification invoke Default on corresponding type, 
             * if true return Default instance. Otherwise invoke Feature member and resolve instance 
             * according to behaviour of IFeatureSpecification implementation.
             */
            var featurespecificationType = typeof (IFeatureSpecification<>)
                .GetGenericTypeDefinition()
                .MakeGenericType(new[] {dependency.TargetItemType});

            var featureSpecification = (IFeatureSpecification<object>) _kernel.Resolve(featurespecificationType);

            return featureSpecification.Default() ? _kernel.Resolve(dependency.TargetItemType) : featureSpecification.Feature();
        }
    }

    internal class ItemType
    {
        internal static Type GetItemType(Type itemType)
        {
            if (itemType == null)
            {
                return null;
            }
            if (itemType.IsArray)
            {
                return itemType.GetElementType();
            }
            if (itemType.IsGenericType == false || itemType.IsGenericTypeDefinition)
            {
                return null;
            }

            return itemType.GetGenericArguments()[0];
        }
    }
}