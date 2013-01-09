using System;
using System.Linq;
using Castle.Core;
using Castle.MicroKernel;

namespace blazey.features
{
    internal class FeatureSpecificationType
    {
        
        private static readonly Type _openGenericType = typeof(IFeatureSpecification<>);
        
        internal static void ThrowIfNotConcrete<T>()
        {
            var implementation = typeof (T);
            if (!implementation.IsAbstract && !implementation.IsInterface) return; //todo: delegate
            const string messageFormat = "Implementation type: '{0}' must be a concrete type.";
            var message = string.Format(messageFormat, implementation.FullName);
            throw new InvalidOperationException(message);
        }

        internal static bool KernelCanResolve(IKernel kernel, DependencyModel dependency)
        {

            if (IsFeatureSpecification(dependency.TargetItemType)) return false;

            var featureSpecificationType = FromFeature(dependency.TargetItemType);

            return kernel.HasComponent(featureSpecificationType);

        }

        internal static bool IsFeatureSpecification(Type toCheck)
        {
            return null != FirstFeatureSpecificationOrDefault(toCheck);
        }

        private static Type FirstFeatureSpecificationOrDefault(Type featureSpecification)
        {
            while (featureSpecification != null && featureSpecification != typeof(object))
            {
                var cur = featureSpecification
                    .GetInterfaces()
                    .Where(type =>
                           type.IsGenericType &&
                           type.GetGenericTypeDefinition() == _openGenericType.GetGenericTypeDefinition())
                    .ToArray();

                if (cur.Any())
                {
                    return cur.FirstOrDefault();
                }
                featureSpecification = featureSpecification.BaseType;
            }
            return null;

        }

        internal static Type ToFeature(Type featureSpecification)
        {
            var feature = FirstFeatureSpecificationOrDefault(featureSpecification);
            return null == feature ? null : feature.GetGenericArguments()[0];
        }

        internal static Type FromFeature(Type type)
        {
            if (type == _openGenericType)
            {
                var message = string.Format("type is not '{0}'", _openGenericType.FullName);
                throw new ArgumentException(message);
            }
            
            return _openGenericType.MakeGenericType(type);

        }
    }
}