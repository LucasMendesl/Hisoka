namespace Hisoka
{
    internal class GenericQueryFormater : Formater
    {
        protected override void ConfigureFormaters()
        {
            FormatMap.Add(FilterType.NotEqual, (property, filterType, paramters) => string.Format(Consts.NotEqualsPredicateFormat, property));
            FormatMap.Add(FilterType.LessThan, (property, filterType, paramters) => string.Format(Consts.SmallerThanPredicateFormat, property));
            FormatMap.Add(FilterType.GreaterThan, (property, filterType, paramters) => string.Format(Consts.GreaterThanPredicateFormat, property));
            FormatMap.Add(FilterType.LessThanOrEqual, (property, filterType, paramters) => string.Format(Consts.SmallerThanOrEqualsPredicateFormat, property));
            FormatMap.Add(FilterType.GreaterThanOrEqual, (property, filterType, paramters) => string.Format(Consts.GreaterThanOrEqualsPredicateFormat, property));
        }
    }
}
