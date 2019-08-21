using System;

namespace Hisoka
{
    /// <summary>
    /// Representa uma exception ocorrida ao realizar uma operação de consulta
    /// </summary>
    [Serializable]
    public class HisokaException : Exception
    {
        /// <summary>
        /// Inicializa uma instância de <see cref="HisokaException"/>
        /// </summary>
        public HisokaException() : base() { }

        /// <summary>
        /// Inicializa uma instância de <see cref="HisokaException" />
        /// </summary>
        /// <param name="message">mensagem da exception</param>
        public HisokaException(string message) : base(message) { }

        /// <summary>
        /// Inicializa uma instância de <see cref="HisokaException" />
        /// </summary>
        /// <param name="message">mensagem da exception</param>
        /// <param name="innerException">exception ocorrida anteriormente</param>
        public HisokaException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
