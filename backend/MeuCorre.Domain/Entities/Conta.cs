using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuCorre.Domain.Entities
{
    public class Conta : Entidade
    {
        // Propriedades obrigatórias
        public new Guid Id { get; set; }
        public new DateTime DataCriacao { get; set; }
        public new DateTime? DataAtualizacao { get; set; }

        // Propriedades da Conta
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty; // Ex: "CartaoCredito", "Carteira"
        public decimal Saldo { get; set; }
        public Guid UsuarioId { get; set; }
        public bool Ativo { get; set; }
        public decimal? Limite { get; set; }
        public int? DiaFechamento { get; set; }
        public int? DiaVencimento { get; set; }
        public string Cor { get; set; } = string.Empty;
        public string Icone { get; set; } = string.Empty;

        // Nova propriedade apenas para cartões de crédito
        public string? TipoLimite { get; set; } // Ex: "Fixo", "Rotativo"

        // Navegação
        public Usuario? Usuario { get; set; }

        // Métodos de domínio
        public bool EhCartaoCredito() => Tipo?.ToLower() == "cartaocredito";
        public bool EhCarteira() => Tipo?.ToLower() == "carteira";

        public decimal CalcularLimiteDisponivel()
        {
            if (!EhCartaoCredito() || Limite == null)
                return 0;

            decimal limiteDisponivel = Limite.Value - Saldo;
            return limiteDisponivel < 0 ? 0 : limiteDisponivel;
        }

        public bool PodeFazerDebito(decimal valor)
        {
            if (!Ativo) return false;

            if (EhCarteira()) return Saldo >= valor;
            if (EhCartaoCredito()) return CalcularLimiteDisponivel() >= valor;

            return false;
        }
    }
}
