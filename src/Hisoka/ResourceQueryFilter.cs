using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Hisoka
{
    /// <summary>
    /// Classe que representa a query composta pelos parametros passados pela url
    /// </summary>
	public class ResourceQueryFilter
    {
        private readonly List<Filter> _filters;
        private readonly List<Sort> _sorts;
        private readonly List<string> _fields;

        /// <summary>
        /// Recupera a coleção de campos para filtros 
        /// </summary>
        public IReadOnlyList<Filter> Filters => _filters.AsReadOnly();

        /// <summary>
        /// Recupera a coleção de campos para ordenação
        /// </summary>
        public IReadOnlyList<Sort> Sorts => _sorts.AsReadOnly();

        /// <summary>
        /// Recupera a coleção de campos para projeção
        /// </summary>
        public IReadOnlyCollection<string> Fields => _fields.AsReadOnly();

        /// <summary>
        /// Recupera os campos referentes a paginação
        /// </summary>
        public Paginate Paginate { get; private set; }

        internal ResourceQueryFilter(string filter, string sorter, string fields, Paginate paginate)
        {
            Paginate = paginate;
            _filters = GetBy<Filter>(filter);
            _sorts = GetBy<Sort>(sorter);
            _fields = fields?.Split(',').ToList() ?? new List<string>();
        }

        /// <summary>
        /// Método responsável por pesquisar um item da coleção de filtros por nome 
        /// </summary>
        /// <param name="propertyName">nome da propriedade a ser pesquisada</param>
        /// <returns>retorna o objeto <see cref="Filter"/> </returns>
        public Filter GetFilter(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentNullException("Parameter propertyName is mandatory!");

            return _filters.FirstOrDefault(x => propertyName.Equals(x.PropertyName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Método responsável por adicionar um filtro na consulta
        /// </summary>
        /// <param name="filter">parâmetros a serem filtrados</param>
        public void AddFilter(Filter filter)
        {
            if (filter == null) throw new ArgumentNullException("Parameter filter is mandatory!");

            _filters.Add(filter);
        }

        /// <summary>
        /// Método responsável por obter um item da coleção dos parâmetros de ordenação
        /// </summary>
        /// <param name="propertyName">nome da propriedade a ser pesquisada</param>
        /// <returns>retorna o objeto <see cref="Sort"/></returns>
        public Sort GetSorter(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentNullException("Parameter propertyName is mandatory!");

            return _sorts.FirstOrDefault(x => propertyName.Equals(x.PropertyName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Método responsável por adicionar uma ordenação na consulta
        /// </summary>
        /// <param name="criteria">critério de ordenação a ser executado</param>
        public void AddSorter(Sort criteria)
        {
            if (criteria == null) throw new ArgumentNullException("Parameter criteria is mandatory!");

            _sorts.Add(criteria);
        }

        /// <summary>
        /// Método responsável por adicionar um campo para a projeção dos dados
        /// </summary>
        /// <param name="field"></param>
        public void AddField(string field)
        {
            if (string.IsNullOrEmpty(field)) throw new ArgumentNullException("Parameter field is mandatory!");

            _fields.Add(field);
        }

        private static List<T> GetBy<T>(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return new List<T>();

            return query.Split(',')
                        .Select(s => (T)Activator.CreateInstance(typeof(T), BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { s }, null))
                        .ToList();
        }
    }
}
