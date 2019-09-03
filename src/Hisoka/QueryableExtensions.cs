using Hisoka;
using System.Reflection;
using System.Collections;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Linq.Dynamic.Core.Exceptions;

namespace System.Linq
{
    /// <summary>
    /// Classe responsável por prover métodos de extensão para as coleções IQueryable
    /// </summary>
    public static partial class QueryableExtensions
    {
        /// <summary>
        /// Método responsável por realizar um conjunto de filtros em uma coleção
        /// </summary>
        /// <typeparam name="T">entidade</typeparam>
        /// <param name="source">fonte dos dados</param>
        /// <param name="filters">filtros a serem executados</param>
        /// <returns>retorna a query filtrada</returns>
        public static IQueryable<T> Filter<T>(this IQueryable<T> source, IEnumerable<Filter> filters)
            where T : class
        {
            if (source == null) { throw new ArgumentException($"{ nameof(source) }"); }

            if (filters == null || !filters.Any())
                return source;

            return new FilterQuery<T>(filters.ToList()).Apply(source);
        }

        /// <summary>
        /// Método responsável por selecionar campos em uma coleção,
        /// obs: os campos passados no parametro fields deverão vir no modelo a seguir:
        /// campo1, campo2, Objeto(campo1, campo2), Colecao[campo1, campo2]
        /// </summary>
        /// <param name="source">fonte de dados</param>
        /// <param name="fields">campos a serem selecionados</param>
        /// <returns>retorna a fonte de dados com apenas os campos selecionados</returns>
        public static IQueryable Project(this IQueryable source, IEnumerable<string> fields)
        {
            if (source == null) { throw new ArgumentException($"{ nameof(source) }"); }

            if (fields == null || !fields.Any())
                return source;

            try
            {
                var config = new ParsingConfig { AllowNewToEvaluateAnyType = true };
                var projection = new ProjectionQueryParser();

                return source.Select(config, projection.ParseValues(fields.ToArray()));
            }
            catch (ParseException e)
            {
                throw new HisokaException("An error occurs when try to parse a querystring parameters in database statement", e);
            }
        }

        /// <summary>
        /// Método responsável por ordenar uma coleção por um ou mais campos 
        /// </summary>
        /// <typeparam name="T">entidade</typeparam>
        /// <param name="source">fonte de dados</param>
        /// <param name="sorters">campos a serem ordenados</param>
        /// <returns>retorna a fonte de dados com os campos ordenados</returns>
        public static IQueryable<T> Sort<T>(this IQueryable<T> source, IEnumerable<Sort> sorters)
             where T : class
        {
            if (source == null) { throw new ArgumentException($"{ nameof(source) }"); }

            if (sorters == null || !sorters.Any())
                return source;

            return new SorterQuery<T>(sorters).Apply(source);
        }

        /// <summary>
        /// Método responsável por realizar uma query (filtro e ordenação)
        /// </summary>
        /// <typeparam name="T">entidade</typeparam>
        /// <param name="source">fonte de dados</param>
        /// <param name="query">parametros para realização da consulta</param>
        /// <returns>retorna a query executada</returns>
        public static IQueryable<T> Query<T>(this IQueryable<T> source, ResourceQueryFilter query)
            where T : class
        {
            return source.Filter(query.Filters)
                         .Sort(query.Sorts);
        }

        /// <summary>
        /// Método responsável por realizar uma query (filtro, ordenação e projeção)
        /// </summary>
        /// <typeparam name="T">entidade</typeparam>
        /// <param name="source">fonte de dados</param>
        /// <param name="query">parametros para realização da consulta</param>
        /// <returns>retorna a query executada</returns>
        public static IQueryable ProjectedQuery<T>(this IQueryable<T> source, ResourceQueryFilter query)
            where T : class
        {
            return source.Filter(query.Filters)
                         .Sort(query.Sorts)
                         .Project(query.Fields.ToArray());
        }

        /// <summary>
        /// Método responsável por realizar paginação em uma lista
        /// </summary>
        /// <typeparam name="T">entidade</typeparam>
        /// <param name="source">fonte de dados</param>
        /// <param name="paginate">parametros referente a paginação</param>
        /// <returns>retorna a consulta paginada</returns>
        public static IPagedList<T> ToPagedList<T>(this IQueryable<T> source, Paginate paginate)
            where T : class
        {
            var count = source.Count();

            var pageNum = GetPageNum(count, paginate.Limit, paginate.Offset);
            var items = source.Skip((pageNum - 1) * paginate.Limit)
                              .Take(paginate.Limit);

            return new PagedList<T>(items, pageNum, paginate.Limit, count);
        }

        /// <summary>
        /// Método responsável por realizar paginação em uma lista de maneira asincrona
        /// </summary>
        /// <typeparam name="T">entidade</typeparam>
        /// <param name="source">fonte de dados</param>
        /// <param name="countAsync">callback para a execução de uma contagem asincrona</param>
        /// <param name="paginate">retorna a lista paginada</param>
        /// <returns></returns>
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(
            this IQueryable<T> source,
            Func<IQueryable<T>, Task<int>> countAsync,
            Paginate paginate
            ) where T : class
        {
            var count = await countAsync(source);
            var pageNum = GetPageNum(count, paginate.Limit, paginate.Offset);

            var page = source.Skip((pageNum - 1) * paginate.Limit).Take(paginate.Limit);
            var items = await page.ToListAsync<T>();

            return new PagedList<T>(items, pageNum, paginate.Limit, count);
        }

        private static int GetPageNum(int count, int pageSize, int pageNum)
        {
            int totalPages = (count + pageSize - 1) / pageSize;
            return pageNum > totalPages ? totalPages : pageNum;
        }

        internal static bool IsEnumerable<TEntity>(this string property)
        {
            var prop = typeof(TEntity).GetProperty(property, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (prop == null)
                throw new HisokaException(string.Format("Property '{0}' is not a member of the taget entity.", property));

            return prop.PropertyType.IsEnumerableType();
        }

        internal static bool IsEnumerableType(this Type type)
        {
            return type != typeof(string) && (typeof(IEnumerable).IsAssignableFrom(type));
        }
    }
}
