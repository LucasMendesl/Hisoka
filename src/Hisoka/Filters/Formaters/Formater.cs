using System;
using System.Text;
using System.Collections.Generic;

namespace Hisoka
{
    internal abstract class Formater
    {
        public Formater()
        {
            ConfigureFormaters();
        }

        protected IDictionary<FilterType, Func<string, FilterType, object[], string>> FormatMap = new Dictionary<FilterType, Func<string, FilterType, object[], string>>
        {
            { FilterType.Equal, (property, filterType, paramters) => string.Format(Consts.EqualsPredicateFormat, property) },
            { FilterType.OrEqual, (property, filterType, paramters) =>
                {
                    var result = new StringBuilder();
                    for (int i = 0; i < paramters.Length; i++)
                    {
                        if (i > 0)
                            result.Append(" || ");

                        result.AppendFormat(Consts.InclusiveOrEqualPredicateFormat, "@" + i.ToString(), property);
                    }
                    return result.ToString();
                }
            }
        };

        protected abstract void ConfigureFormaters();

        public string FormatPredicate(string property, FilterType op, params object[] values)
        {
            if (FormatMap.TryGetValue(op, out var callback))
            {
                return callback(property, op, values);
            }

            throw new NotSupportedException(string.Format("Operator {0} is Not supported", op));
        }

    }
}
