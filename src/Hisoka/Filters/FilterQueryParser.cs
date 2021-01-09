using System;
using System.Linq;
using Hisoka.Configuration;

namespace Hisoka
{
    class FilterQueryParser<T> : IQueryParser<T>
        where T : class
    {
        private readonly Filter filter;
        private readonly Formater formater;

        public FilterQueryParser(Filter filter, Formater formater)
        {
            this.filter = filter;
            this.formater = formater;
        }

        public string ParseValues(object[] values)
        {
            string propName = null;
            string result = null;

            var propArray = filter.PropertyName.Split('.');
            var op = filter.Operator;
            
            var metadata = HisokaConfiguration.GetPropertyMetadataFromCache(typeof(T), propArray[0]);
            var property = metadata.CurrentProperty;

            try
            {
                if (propArray[0].IsEnumerable<T>())
                {
                    if (propArray.Length != 2) 
                    {
                        throw new HisokaException("Unsupporter array argument lenght.");
                    }

                    if (!property.PropertyType.IsGenericType) 
                    {
                        propName = propArray[1];
                    }
                    else 
                    {
                        var argument = property.PropertyType.GenericTypeArguments[0];
                        var cacheValue = HisokaConfiguration.GetPropertyMetadataFromCache(argument, propArray[1]); 
                        propName = cacheValue.CurrentProperty.Name;
                    }

                    result = string.Format(Consts.ArrayPredicateFormat, property.Name, formater.FormatPredicate(propName, op, values));
                }
                else
                {
                    propName = filter.PropertyName;
                    result = formater.FormatPredicate(propName, op, values);
                }
            }
            catch (NotSupportedException ex)
            {
                throw new HisokaException(string.Format("The operand '{0}' is not supported for property '{1}'.", op, propName), ex);
            }

            return result;
        }
    }
}
