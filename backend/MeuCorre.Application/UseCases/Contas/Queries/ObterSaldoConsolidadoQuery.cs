using MediatR;
using System;

namespace MeuCorre.Application.UseCases.Contas.Queries
{
    public class ObterSaldoConsolidadoQuery : IRequest<SaldoConsolidadoDto>
    {
        public required Guid UsuarioId { get; set; }
    }

    public class SaldoConsolidadoDto
    {
        public decimal DinheiroDisponivel { get; set; }
        public decimal LimiteCartaoDisponivel { get; set; }
        public DetalheSaldoPorTipoDto Detalhes { get; set; } = new();
    }

    public class DetalheSaldoPorTipoDto
    {
        public decimal ContasBancarias { get; set; }
        public decimal Carteiras { get; set; }
        public decimal CartoesCredito { get; set; }
    }
}
