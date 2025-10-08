using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuCorre.Application.UseCases.Contas.Responses
{
    public class ContaResumoResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
        public bool Ativo { get; set; }
        public decimal? LimiteDisponivel { get; set; }
    }
}
