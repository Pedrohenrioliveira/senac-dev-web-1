using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuCorre.Domain.Enums
{
    /// <summary>
    /// Enumeração que representa o tipo de limite para cartões de crédito.
    /// </summary>
    public enum TipoLimite
    {
        /// <summary>
        /// Limite total disponível no cartão de crédito.
        /// </summary>
        Total = 1,

        /// <summary>
        /// Limite mensal disponível no cartão de crédito.
        /// </summary>
        Mensal = 2
    }
}
