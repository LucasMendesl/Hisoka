using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace System.Linq
{
    /// <summary>
    /// Classe responsável por prover métodos de extensão para as coleções IEnumerable
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Realiza a conversão para uma instância de <see cref="List{T}" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">fonte de dados</param>
        /// <returns>retorna uma lista</returns>
        public static List<T> ToList<T>(this IEnumerable source)
        {
            return source.Cast<T>().ToList();
        }

        /// <summary>
        /// Realiza a conversão para uma instância de <see cref="List{T}" /> de maneira asincrona
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">fonte de dados</param>
        /// <returns>retorna uma lista</returns>
        public static Task<List<T>> ToListAsync<T>(this IEnumerable source)
        {
            return Task.Run(() => source.ToList<T>());
        }
    }
}
