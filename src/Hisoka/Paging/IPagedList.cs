using System.Collections.Generic;

namespace Hisoka
{
    /// <summary>
    /// Abstração referente a uma coleção paginada
    /// </summary>
    /// <typeparam name="T">Entidade</typeparam>
    public interface IPagedList<T>
        where T : class
    {
        /// <summary>
        /// Representa os dados da lista
        /// </summary>
        IList<T> Data { get; }

        /// <summary>
        /// Representa o número da página atual
        /// </summary>
        int PageNumber { get; }

        /// <summary>
        /// Representa a quantidade total de páginas da coleção
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// Representa o tamanho da página
        /// </summary>
        int TotalPages { get; }

        /// <summary>
        /// Representa a quantidade total de registros da coleção
        /// </summary>
        int TotalCount { get; }

        /// <summary>
        /// Verifica se a coleção possui uma página anterior
        /// </summary>
        bool HasPrevious { get; }

        /// <summary>
        /// Verifica se a coleção possui uma próxima página
        /// </summary>
        bool HasNext { get; }
    }
}
