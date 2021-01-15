using System;
using System.Linq;
using System.ComponentModel;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using Hisoka.Configuration;

namespace Hisoka
{
    class FilterQuery<TEntity> : IQuery<TEntity>
        where TEntity : class
    {
        private readonly IEnumerable<Filter> filters;

        public FilterQuery(IEnumerable<Filter> filter)
        {
            this.filters = filter;
        }

        public IQueryable<TEntity> Apply(IQueryable<TEntity> source)
        {
            if (null == filters || !filters.Any())
                return source;

            foreach (var filter in filters)
            {
                var targetMetadata = GetTargetMetadata(filter);

                if (!targetMetadata.AllowFilter) continue;
                
                var targetType = targetMetadata.CurrentProperty;
                var values = ConvertFromStringValues(filter, targetType.PropertyType);
                var predicate = new FilterQueryParser<TEntity>(filter, GetFormater(targetType.PropertyType));

                source = source.Where(predicate.ParseValues(values), values);
            }

            return source;
        }

        private Formater GetFormater(Type type)
        {
            if (typeof(string) == type)
                return new StringQueryFormater();

            return new GenericQueryFormater();
        }

        private HisokaPropertyMetadata GetTargetMetadata(Filter filter)
        { 
            var propType = typeof(TEntity);        
            var propArray = filter.PropertyName.Split('.');

            HisokaPropertyMetadata propertyMetadata = null;

            foreach (var name in propArray)
            {
                if (propType.IsEnumerableType())
                {
                    if (propType.IsGenericType)
                        propType = propType.GenericTypeArguments[0];
                    else if (propType.IsArray)
                        propType = propType.GetElementType();
                }

                propertyMetadata = HisokaConfiguration.GetPropertyMetadataFromCache(propType, name);

                if (null == propertyMetadata)
                    throw new HisokaException(string.Format("Property '{0}' is not a member of the target entity.", name));
            }

            return propertyMetadata;
        }

        private object[] ConvertFromStringValues(Filter filter, Type targetType)
        {
            if (null != targetType)
            {
                var converter = TypeDescriptor.GetConverter(targetType);
                if (converter.CanConvertFrom(typeof(string)))
                {
                    var result = new List<object>(filter.PropertyValues.Length);
                    foreach (var s in filter.PropertyValues)
                    {
                        if (s == null || "NULL".Equals(s, StringComparison.OrdinalIgnoreCase))
                        {
                            result.Add(null);
                            continue;
                        }

                        var value = converter.ConvertFromString(s);
                        result.Add(value);
                    }

                    return result.ToArray();
                }
            }

            return filter.PropertyValues.Cast<object>().ToArray();
        }
    }
}
