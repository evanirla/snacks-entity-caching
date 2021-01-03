using Microsoft.Extensions.DependencyInjection;
using Snacks.Entity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snacks.Entity.Caching.Extensions
{
    public static class ServiceCollectionExtensions
    {
        static IEnumerable<Type> EntityServiceTypes =>
            AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => !x.FullName.Contains("Snacks.Entity.Core"))
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(IEntityService).IsAssignableFrom(x))
                .Where(x => !x.IsAbstract && !x.IsInterface);

        public static IServiceCollection AddEntityCacheServices(this IServiceCollection services, Action<EntityCacheOptions> setupAction = null)
        {
            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            foreach (Type serviceType in EntityServiceTypes)
            {
                Type modelType = GetModelTypeFromServiceType(serviceType);
                services.AddSingleton(typeof(IEntityCacheService<>).MakeGenericType(modelType),
                    typeof(EntityCacheService<>).MakeGenericType(modelType));
            }

            return services;
        }

        private static Type GetModelTypeFromServiceType(Type serviceType)
        {
            Type modelType = serviceType.BaseType.GetGenericArguments().FirstOrDefault();

            if (modelType == null)
            {
                modelType = serviceType.GetInterface("IEntityService`1").GetGenericArguments().FirstOrDefault();
            }

            return modelType;
        }
    }
}
