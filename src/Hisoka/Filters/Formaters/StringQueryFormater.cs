using System;
using System.Linq;
using System.Text;

namespace Hisoka
{
    internal class StringQueryFormater : Formater
    {
        protected override void ConfigureFormaters()
        {
            FormatMap.Add(FilterType.OrLike, AndOrLike());
            FormatMap.Add(FilterType.NotEqual, NotEqual());
            FormatMap.Add(FilterType.AndLike, AndOrLike());
            FormatMap.Add(FilterType.Contains, (property, filterType, paramters) => string.Format(Consts.LikePredicateFormat, property));
            FormatMap.Add(FilterType.EndsWith, (property, filterType, paramters) => string.Format(Consts.EndsWithPredicateFormat, property));
            FormatMap.Add(FilterType.StartsWith, (property, filterType, paramters) => string.Format(Consts.StartsWithPredicateFormat, property));
        }

        private Func<string, FilterType, object[], string> NotEqual()
        {
            return (property, filterType, paramters) =>
            {
                var strValue = paramters.First() as string;
                return string.IsNullOrEmpty(strValue) ?
                    string.Format(Consts.IsNullOrEmptyPredicateFormat, property) :
                    string.Format(Consts.NotEqualsPredicateFormat, property);
            };
        }

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
