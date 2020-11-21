namespace Hisoka
{
    public static class QueryFilterOptions
    {
        public static int MaxPageSize { get; set; }
        public static int DefaultPageSize { get; set; }
        public static string SelectFieldsQueryAlias { get; set; }
        public static string OrderByQueryAlias { get; set; }
        public static string PageNumberQueryAlias { get; set; }
        public static string PageSizeQueryAlias { get; set; }

        public static void ApplyConfig(HisokaOptions options)
        {
            MaxPageSize = options.MaxPageSize;
            DefaultPageSize = options.DefaultPageSize;
            SelectFieldsQueryAlias = options.SelectFieldsQueryAlias;
            OrderByQueryAlias = options.OrderByQueryAlias;
            PageNumberQueryAlias = options.PageNumberQueryAlias;
            PageSizeQueryAlias = options.PageSizeQueryAlias;
        }
    }
}