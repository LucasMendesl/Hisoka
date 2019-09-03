using System.Linq;
using System.Collections.Generic;

namespace Hisoka
{
    /// <summary>
    /// Classe que representa uma coleção de dados paginada
    /// </summary>
    /// <typeparam name="T">Entidade</typeparam>
    public class PagedList<T> : IPagedList<T>
        where T : class
    {

        /// <summary>
        /// Representa os dados da lista
        /// </summary>
        public IList<T> Data { get; private set; }


        /// <summary>
        /// Representa o número da página atual
        /// </summary>
        public int PageNumber { get; private set; }

        /// <summary>
        /// Representa a quantidade total de páginas da coleção
        /// </summary>
        public int TotalPages { get; private set; }

        /// <summary>
        /// Representa o tamanho da página
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// Representa a quantidade total de registros da coleção
        /// </summary>
        public int TotalCount { get; private set; }

        /// <summary>
        /// Verifica se a coleção possui uma página anterior
        /// </summary>
        public bool HasPrevious
        {
            get { return PageNumber > 1; }
        }

        /// <summary>
        /// Verifica se a coleção possui uma próxima página
        /// </summary>
        public bool HasNext
        {
            get { return PageNumber < TotalPages; }
        }

        /// <summary>
        /// Inicializa uma instância da classe <see cref="PagedList{T}" />
        /// </summary>
        /// <param name="items">coleção de itens a serem paginados</param>
        /// <param name="pageNumber">numero da página</param>
        /// <param name="pageSize">tamanho da página</param>
        /// <param name="count">quantidade de registros da coleção</param>
        public PagedList(IEnumerable<T> items, int pageNumber, int pageSize, int totalCount)
        {
            Data = items.ToList();
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (totalCount + pageSize - 1) / pageSize;
        }
    }
}
