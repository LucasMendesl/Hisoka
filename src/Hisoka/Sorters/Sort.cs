using System;
using System.Collections.Generic;

namespace Hisoka
{
    /// <summary>
    /// Classe que representa a ordenação dinâmica de url 
    /// </summary>
	public class Sort
    {
        private readonly IDictionary<SortType, string> directionMap = new Dictionary<SortType, string>
        {
            { SortType.Ascending, "ASC" },
            { SortType.Descending, "DESC" }
        };

        /// <summary>
        /// Representa o nome do campo a ser ordenado
        /// </summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// Representa o tipo de ordenação (asc / desc)
        /// </summary>
        public SortType Direction { get; private set; }

        internal Sort(string queryStringValues)
        {
            if (string.IsNullOrEmpty(queryStringValues)) throw new ArgumentNullException("invalid internal parameter, please verify if url has a sort parameter");

            PropertyName = queryStringValues[0] == '-' ? queryStringValues.Substring(1) : queryStringValues;
            Direction = queryStringValues[0] == '-' ? SortType.Descending : SortType.Ascending;
        }

        /// <summary>
        /// Inicializa uma instância da classe <see cref="Sort" />
        /// </summary>
        /// <param name="propertyName">nome do campo a ser ordenado</param>
        /// <param name="direction">tipo de ordenação</param>
        public Sort(string propertyName, SortType direction)
        {
            PropertyName = propertyName;
            Direction = direction;
        }

        /// <summary>
        /// Retorna a instância dessa classe como uma String, nenhuma conversão é executada
        /// </summary>
        /// <returns>representação do objeto em uma string</returns>
        public override string ToString()
        {
            return $"{PropertyName} {directionMap[Direction]}";
        }
    }
}
