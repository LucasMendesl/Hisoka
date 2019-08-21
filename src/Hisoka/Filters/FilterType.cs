namespace Hisoka
{
    /// <summary>
    /// Enumeração responsável pelo tipo de filtro de uma query
    /// </summary>
	public enum FilterType : byte
    {
        /// <summary>
        /// Representa o filtro .Where(x => x.a == "a" || x.b == "n"... )
        /// </summary>
        OrEqual,


        /// <summary>
        /// Representa o filtro .Where(x => x.Contains("a") || x.Contains("b") ...)
        /// </summary>
        OrLike,

        /// <summary>
        /// Representa o filtro .Where(x => x.Contains("a") &amp;&amp; x.Contains("b") ...)
        /// </summary>
        AndLike,

        /// <summary>
        /// Representa o filtro igual
        /// </summary>
		Equal,

        /// <summary>
        /// Representa o filtro diferente
        /// </summary>
		NotEqual,

        /// <summary>
        /// Representa o filtro maior que (&gt;)
        /// </summary>
		GreaterThan,

        /// <summary>
        /// Representa o filtro menor que (&lt;)
        /// </summary>
		LessThan,

        /// <summary>
        /// Representa o filtro maior ou igual (&gt;=)
        /// </summary>
		GreaterThanOrEqual,

        /// <summary>
        /// Representa o filtro menor ou igual (&lt;=)
        /// </summary>
		LessThanOrEqual,

        /// <summary>
        /// Representa o filtro Contains
        /// </summary>
		Contains,

        /// <summary>
        /// Representa o filtro Começa com
        /// </summary>
		StartsWith,


        /// <summary>
        /// Representa o filtro Termina com
        /// </summary>
		EndsWith
    }
}
