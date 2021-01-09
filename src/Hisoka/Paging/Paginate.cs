using Hisoka.Configuration;

namespace Hisoka
{
    /// <summary>
    /// Classe que representa os dados de paginação
    /// </summary>
    public class Paginate
    {
        private int _pageNumber = 1;

        /// <summary>
        /// Representa a página atual 
        /// </summary>
        public int Offset
        {
            get { return _pageNumber; }
            set
            {
                if (value >= 1) { _pageNumber = value; }
            }
        }

        private int _pageSize = HisokaConfiguration.DefaultPageSize;

        /// <summary>
        /// Representa a quantidade de registros por página
        /// </summary>
        public int Limit
        {
            get { return _pageSize; }
            set
            {
                if (value <= HisokaConfiguration.MaxPageSize &&
                    value > 0)
                {
                    _pageSize = value;
                }
            }
        }

        internal Paginate(int offset, int limit)
        {
            Offset = offset;
            Limit = limit;
        }
    }
}
