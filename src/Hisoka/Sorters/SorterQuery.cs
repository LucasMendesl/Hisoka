using System.Linq;
using Hisoka.Configuration;
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

            var cacheKey = typeof(TEntity);
            var translatedItemList = new List<Sort>(sorts.Count());

            foreach (var sort in sorts) 
            {
                var cacheItem = HisokaConfiguration.GetPropertyMetadataFromCache(cacheKey, sort.PropertyName);
                translatedItemList.Add(new Sort(cacheItem.CurrentProperty.Name, sort.Direction));
            }

            var ordering = string.Join(",", translatedItemList.Select(s => s.ToString()));
            return source.OrderBy(ordering);
        }
    }
}