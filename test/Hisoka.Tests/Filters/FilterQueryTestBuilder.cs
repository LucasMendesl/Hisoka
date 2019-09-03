using System.Linq;
using System.Collections.Generic;

namespace Hisoka.Tests.Filters
{
    class FilterQueryTestBuilder<T>
        where T : class
    {
        private IQueryable<T> _collection;

        private readonly List<Filter> filters;

        public FilterQueryTestBuilder()
        {
            filters = new List<Filter>();
        }

        public FilterQueryTestBuilder<T> WithFilter(string property, FilterType filterType, params string[] values)
        {
            filters.Add(new Filter(property, filterType, values));
            return this;
        }

        public FilterQueryTestBuilder<T> InCollection(IQueryable<T> collection)
        {
            _collection = collection;
            return this;
        }

        public List<T> Build()
        {
            return new FilterQuery<T>(filters).Apply(_collection).ToList();
        }

    }
}
