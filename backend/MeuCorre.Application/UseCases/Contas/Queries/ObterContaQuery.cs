using MediatR;
using MeuCorre.Domain.Entities;
using MeuCorre.Domain.Interfaces.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace MeuCorre.Application.UseCases.Contas.Queries
{
    public class ObterContaQuery : IRequest<ContaDetalheDto>
    {
        [Required(ErrorMessage = "Informe o Id da conta")]
        public required Guid ContaId { get; set; }

        [Required(ErrorMessage = "Informe o Id do usuário")]
        public required Guid UsuarioId { get; set; }
    }

    internal class ObterContaQueryHandler : IRequestHandler<ObterContaQuery, ContaDetalheDto>
    {
        private readonly IContaRepository<Conta> _contaRepository;

        public ObterContaQueryHandler(IContaRepository<Conta> contaRepository)
        {
            _contaRepository = contaRepository;
        }

        public async Task<ContaDetalheDto> Handle(ObterContaQuery request, CancellationToken cancellationToken)
        {
            var conta = await _contaRepository.ObterPorIdEUsuarioAsync(request.ContaId, request.UsuarioId);

            var contaDto = new ContaDetalheDto
            {
                Id = conta?.Id ?? Guid.Empty,
                Nome = conta?.Nome ?? string.Empty,
                Tipo = conta?.Tipo ?? string.Empty,
                Saldo = conta?.Saldo ?? 0,
                Ativo = conta?.Ativo ?? false,
                LimiteDisponivel = conta != null && conta.EhCartaoCredito() ? (conta.Limite ?? 0) - conta.Saldo : null,
                QuantidadeTransacoes = 0,
                TotalReceitas = 0,
                TotalDespesas = 0
            };

            return contaDto;
        }
    }

    public class ContaDetalheDto
    {
        public Guid Id { get; set; }
        public required string Nome { get; set; }
        public required string Tipo { get; set; }
        public decimal Saldo { get; set; }
        public bool Ativo { get; set; }
        public decimal? LimiteDisponivel { get; set; }
        public int QuantidadeTransacoes { get; set; }
        public decimal TotalReceitas { get; set; }
        public decimal TotalDespesas { get; set; }
    }
}
