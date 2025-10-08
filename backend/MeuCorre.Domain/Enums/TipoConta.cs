using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuCorre.Domain.Enums
{  
 /// <summary>
 /// Enumeração que representa os tipos de conta disponíveis no sistema.
 /// </summary>
    public enum TipoConta
    {
     /// <summary>
     /// Carteira física ou digital utilizada para movimentações rápidas do dia a dia.
     /// </summary>
     Carteira = 1,

     /// <summary>
     /// Conta bancária tradicional, como conta corrente ou poupança.
     /// </summary>
     ContaBancaria = 2,

     /// <summary>
     /// Cartão de crédito utilizado para compras parceladas ou a prazo.
     /// </summary>
     CartaoCredito = 3
    }

}
