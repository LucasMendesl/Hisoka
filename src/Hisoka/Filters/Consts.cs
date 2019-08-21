namespace Hisoka
{
    internal class Consts
    {
        public const string ArrayPredicateFormat = "{0}.Any({1})";

        public const string NotEqualsPredicateFormat = "{0} != @0";
        public const string EqualsPredicateFormat = "{0} == @0";
        public const string GreaterThanPredicateFormat = "null != {0} && {0} > @0";
        public const string GreaterThanOrEqualsPredicateFormat = "null != {0} && {0} >= @0";
        public const string SmallerThanPredicateFormat = "null != {0} && {0} < @0";
        public const string SmallerThanOrEqualsPredicateFormat = "null != {0} && {0} <= @0";

        public const string InclusiveOrEqualPredicateFormat = "{0} == {1}";
        public const string InclusiveOrLikePredicateFormat = "{1}.Contains({0})";

        public const string LikePredicateFormat = "{0}.Contains(@0)";
        public const string IsNullOrEmptyPredicateFormat = "!({0} == \"\" || {0} == null)";
        public const string StartsWithPredicateFormat = "{0}.StartsWith(@0)";
        public const string EndsWithPredicateFormat = "{0}.EndsWith(@0)";
    }
}
