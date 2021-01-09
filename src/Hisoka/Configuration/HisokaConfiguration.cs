using System;
using System.Collections.Generic;
using System.Collections.Concurrent;


namespace Hisoka.Configuration
{
    internal static class HisokaConfiguration
    {
        private static ConcurrentDictionary<Type, Dictionary<string, HisokaPropertyMetadata>> _hisokaConfigurationCache = new ConcurrentDictionary<Type, Dictionary<string, HisokaPropertyMetadata>>();

        public static int MaxPageSize { get; set; }
        public static int DefaultPageSize { get; set; }
        public static string SelectFieldsQueryAlias { get; set; }
        public static string OrderByQueryAlias { get; set; }
        public static string PageNumberQueryAlias { get; set; }
        public static string PageSizeQueryAlias { get; set; }

        internal static void ApplyConfig(HisokaOptions options)
        {
            MaxPageSize = options.MaxPageSize;
            DefaultPageSize = options.DefaultPageSize;
            SelectFieldsQueryAlias = options.SelectFieldsQueryAlias;
            OrderByQueryAlias = options.OrderByQueryAlias;
            PageNumberQueryAlias = options.PageNumberQueryAlias;
            PageSizeQueryAlias = options.PageSizeQueryAlias;
        }

        internal static IDictionary<Type, Dictionary<string, HisokaPropertyMetadata>> GetCache() 
        {
            return _hisokaConfigurationCache;
        }

        internal static HisokaPropertyMetadata GetPropertyMetadataFromCache(Type cacheStoreKey, string propertyAlias) 
        {
            if (_hisokaConfigurationCache.TryGetValue(cacheStoreKey, out var metadataMap)) 
            {
                if (metadataMap.TryGetValue(propertyAlias.ToLower(), out var metadata)) 
                {
                    return metadata;
                }

                return null;
            }

            throw new HisokaException($"the type '{cacheStoreKey.Name}' is not registred");  
        }

        internal static void StorePropertyMetadataInCache(Type cacheKey, Dictionary<string, HisokaPropertyMetadata> metadataMap) 
        {
            _hisokaConfigurationCache.TryAdd(cacheKey, metadataMap);
        }       
    }
}