using System;
using System.Text;
using System.Collections.Generic;

namespace Hisoka
{
    internal class GenericQueryFormater : Formater
    {
        protected override IDictionary<FilterType, Func<string, FilterType, object[], string>> FormatMap => new Dictionary<FilterType, Func<string, FilterType, object[], string>>
        {
            { FilterType.NotEqual, (property, filterType, paramters) => string.Format(Consts.NotEqualsPredicateFormat, property) },
            { FilterType.Equal, (property, filterType, paramters) => string.Format(Consts.EqualsPredicateFormat, property) },
            { FilterType.GreaterThan, (property, filterType, paramters) => string.Format(Consts.GreaterThanPredicateFormat, property) },
            { FilterType.GreaterThanOrEqual, (property, filterType, paramters) => string.Format(Consts.GreaterThanOrEqualsPredicateFormat, property) },
            { FilterType.LessThan, (property, filterType, paramters) => string.Format(Consts.SmallerThanPredicateFormat, property) },
            { FilterType.LessThanOrEqual, (property, filterType, paramters) => string.Format(Consts.SmallerThanOrEqualsPredicateFormat, property) },
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
    }
}
