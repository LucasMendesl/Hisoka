using System;
using System.Linq;

namespace Hisoka
{
    class FilterQueryParser<T>
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

            try
            {
                if (propArray[0].IsEnumerable<T>())
                {
                    if (propArray.Length != 2)
                        throw new HisokaException("Unsupporter array argument lenght.");

                    propName = propArray[1];
                    result = string.Format(Consts.ArrayPredicateFormat, propArray[0], formater.FormatPredicate(propName, op, values));
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
