namespace Hisoka
{

    /// <summary>
    /// Classe que representa as configurações do hisoka
    /// </summary>
    public class HisokaOptions
    {
        private int _defaultPageSize = 15;
        private int _maxPageSize = 25;

        /// <summary>
        /// Recupera ou define o tamanho máximo para uma página
        /// </summary>
        public int MaxPageSize
        {
            get { return _maxPageSize; }
            set
            {
                if (value > 0) { _maxPageSize = value; }
            }
        }

        /// <summary>
        /// Recupera ou define o tamanho padrão de uma página
        /// </summary>
        public int DefaultPageSize
        {
            get { return _defaultPageSize; }
            set
            {
                if (value > 0) { _defaultPageSize = value; }
            }
        }

        /// <summary>
        /// Recupera ou define o nome do campo querystring para executar a projeção
        /// </summary>
        public string SelectFieldsQueryAlias { get; set; } = "fields";

        /// <summary>
        /// Recupera ou define o nome do campo querystring para executar a ordenação
        /// </summary>
        public string OrderByQueryAlias { get; set; } = "sort_by";

        /// <summary>
        /// Recupera ou define o nome do campo querystring para o número da página
        /// </summary>
        public string PageNumberQueryAlias { get; set; } = "offset";

        /// <summary>
        /// Recupera ou define o nome do campo querystring para o tamanho da página
        /// </summary>
        public string PageSizeQueryAlias { get; set; } = "limit";
    }
}
