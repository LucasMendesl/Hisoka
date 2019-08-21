using System;
using System.Collections.Generic;

namespace Hisoka
{
    internal abstract class Formater
    {
        protected abstract IDictionary<FilterType, Func<string, FilterType, object[], string>> FormatMap { get; }

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
