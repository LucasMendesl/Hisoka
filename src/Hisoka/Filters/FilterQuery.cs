using System;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;

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
                var targetType = GetTargetType(filter);
                var values = ConvertFromStringValues(filter, targetType);
                var predicate = new FilterQueryParser<TEntity>(filter, GetFormater(targetType));

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

        private Type GetTargetType(Filter filter)
        {
            var propType = typeof(TEntity);
            var propArray = filter.PropertyName.Split('.');

            foreach (var name in propArray)
            {
                if (propType.IsEnumerableType())
                {
                    if (propType.IsGenericType)
                        propType = propType.GenericTypeArguments[0];
                    else if (propType.IsArray)
                        propType = propType.GetElementType();
                }

                var p = propType.GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                if (null == p)
                    throw new HisokaException(string.Format("Property '{0}' is not a member of the target entity.", name));

                propType = p.PropertyType;
            }

            return propType;
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
