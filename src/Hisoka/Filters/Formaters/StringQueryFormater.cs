using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Hisoka
{
    internal class StringQueryFormater : Formater
    {
        protected override IDictionary<FilterType, Func<string, FilterType, object[], string>> FormatMap => new Dictionary<FilterType, Func<string, FilterType, object[], string>>
        {
            { FilterType.Contains, (property, filterType, paramters) => string.Format(Consts.LikePredicateFormat, property) },
            { FilterType.Equal, (property, filterType, paramters) => string.Format(Consts.EqualsPredicateFormat, property) },
            { FilterType.StartsWith, (property, filterType, paramters) => string.Format(Consts.StartsWithPredicateFormat, property) },
            { FilterType.EndsWith, (property, filterType, paramters) => string.Format(Consts.EndsWithPredicateFormat, property) },
            { FilterType.NotEqual, (property, filterType, paramters) =>
                {
                    var strValue = paramters.First() as string;
                    return string.IsNullOrEmpty(strValue) ?
                        string.Format(Consts.IsNullOrEmptyPredicateFormat, property) :
                        string.Format(Consts.NotEqualsPredicateFormat, property);
                }
            },
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
            },
            { FilterType.AndLike, AndOrLike() },
            { FilterType.OrLike, AndOrLike() },
        };

        private Func<string, FilterType, object[], string> AndOrLike()
        {
            return (property, filterType, paramters) =>
            {
                var exp = filterType == FilterType.OrLike ? " || " : " && ";
                var result = new StringBuilder();
                for (int i = 0; i < paramters.Length; i++)
                {
                    if (i > 0)
                        result.Append(exp);

                    result.AppendFormat(Consts.InclusiveOrLikePredicateFormat, "@" + i.ToString(), property);
                }

                return result.ToString();
            };
        }
    }

}
