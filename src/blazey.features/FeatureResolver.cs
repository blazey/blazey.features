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

            var f = FeatureSpecificationType.KernelCanResolve(_kernel, dependency);
            return f;
        }

        public object Resolve(CreationContext context, ISubDependencyResolver contextHandlerResolver,
                              ComponentModel model, DependencyModel dependency)
        {
            /*
             * TODO: If type has corrsponding IFeatureSpecification invoke Default on corresponding type, 
             * if true return Default instance. Otherwise invoke Feature member and resolve instance 
             * according to behaviour of IFeatureSpecification implementation.
             */

            var featurespecificationType = FeatureSpecificationType
                .FromFeature(dependency.TargetItemType)
                .FeatureSpecifactionType;

            var featureSpecification = (IFeatureSpecification<object>) _kernel.Resolve(featurespecificationType);

            if (featureSpecification.On()) return _kernel.Resolve(dependency.TargetItemType);
            else return featureSpecification.Feature();
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