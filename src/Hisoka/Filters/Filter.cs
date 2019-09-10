using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Hisoka
{
    /// <summary>
    /// Classe que representa um filtro dinâmico de url
    /// </summary>
	public class Filter
    {
        private static readonly Dictionary<string, FilterType> operators = new Dictionary<string, FilterType>
        {
            { ":eq", FilterType.Equal },
            { ":neq", FilterType.NotEqual },
            { ":gt", FilterType.GreaterThan },
            { ":lt", FilterType.LessThan },
            { ":gte", FilterType.GreaterThanOrEqual },
            { ":lte", FilterType.LessThanOrEqual },
            { ":contains", FilterType.Contains },
            { ":starts_with", FilterType.StartsWith },
            { ":ends_with", FilterType.EndsWith },
            { ":or_like", FilterType.OrLike },
            { ":and_like", FilterType.AndLike },
            { ":or_equal", FilterType.OrEqual }
        };

        /// <summary>
        /// Representa o nome da propriedade a ser filtrada
        /// </summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// Representa o valor da propriedade a ser filtrado
        /// </summary>
        public string[] PropertyValues { get; private set; }

        /// <summary>
        /// Representa o tipo de operação de pesquisa a ser executada
        /// </summary>
        public FilterType Operator { get; private set; }

        internal Filter(string queryStringFilter)
        {
            var param = GetQueryParams(queryStringFilter);

            PropertyName = param.Key;
            PropertyValues = param.Value.Split(';').Select(property => property.Trim()).ToArray();
            Operator = GetOperator(queryStringFilter);
        }

        /// <summary>
        /// Inicializa uma instância de Filter
        /// </summary>
        /// <param name="propertyName">nome da propriedade a ser filtrada</param>
        /// <param name="queryOperator">operação de pesquisa a ser executada</param>
        /// <param name="propertyValues">valores da propriedade a serem filtrados</param>
        public Filter(string propertyName, FilterType queryOperator, params string[] propertyValues)
        {
            PropertyName = propertyName;
            PropertyValues = propertyValues;
            Operator = queryOperator;
        }

        static FilterType GetOperator(string queryStringFilter)
        {
            if (string.IsNullOrWhiteSpace(queryStringFilter))
                throw new ArgumentNullException("Please specify a key for the filter operation.");

            var currentOperator = queryStringFilter.Substring(queryStringFilter.IndexOf(':'), queryStringFilter.IndexOf('=') - queryStringFilter.IndexOf(':'));

            if (operators.TryGetValue(currentOperator, out var filterType))
            {
                return filterType;
            }

            throw new InvalidOperationException($"'{queryStringFilter}' does not contain a valid {nameof(FilterType)}.");
        }

        static KeyValuePair<string, string> GetQueryParams(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                throw new ArgumentNullException("querystring parameter is mandatory");

            KeyValuePair<string, string> keyVault;

            foreach (var op in operators)
            {
                int index = data.IndexOf(op.Key, StringComparison.Ordinal);

                if (index == -1)
                    continue;

                keyVault = new KeyValuePair<string, string>(data.Substring(0, index), data.Substring(data.IndexOf('=') + 1));
            }

            if (string.IsNullOrEmpty(keyVault.Key) || string.IsNullOrEmpty(keyVault.Value))
                throw new InvalidOperationException($"'{data}' is missing an operand.");

            return keyVault;
        }
    }
}
