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
            if (!implementation.IsAbstract && !implementation.IsInterface) return;
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

        static bool IsFeatureSpecification(Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.GetInterfaces()
                                 .Any(x =>
                                      x.IsGenericType &&
                                      x.GetGenericTypeDefinition() == _openGenericType.GetGenericTypeDefinition());

                if(cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }

        internal static Type FromFeature(Type type)
        {
            if (type == _openGenericType)
            {
                var featureSpecifcationFullName = FeatureSpecifcationFullName();
                var message = string.Format("type is not '{0}'", featureSpecifcationFullName);
                throw new ArgumentException(message);
            }
            
            return _openGenericType.MakeGenericType(type);

        }

        private static string FeatureSpecifcationFullName()
        {
            var featureSpecifcationFullName = _openGenericType.FullName;
            return featureSpecifcationFullName;
        }
    }
}