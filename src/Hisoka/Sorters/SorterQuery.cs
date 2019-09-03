using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;

namespace Hisoka
{
    class SorterQuery<TEntity> : IQuery<TEntity>
        where TEntity : class
    {
        private readonly IEnumerable<Sort> sorts;

        public SorterQuery(IEnumerable<Sort> sorts)
        {
            this.sorts = sorts;
        }

        public IQueryable<TEntity> Apply(IQueryable<TEntity> source)
        {
            if (sorts != null && !sorts.Any())
                return source;

            var ordering = string.Join(",", sorts.Select(s => s.ToString()));

            return source.OrderBy(ordering);
        }
    }
}