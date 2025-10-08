using System.ComponentModel.DataAnnotations;
using MediatR;
using MeuCorre.Application.UseCases.Categorias.Dtos;
using MeuCorre.Application.UseCases.Contas.Queries;
using MeuCorre.Domain.Entities;
using MeuCorre.Domain.Interfaces.Repositories;

namespace MeuCorre.Application.UseCases.Categorias.Queries
{
    public class ObterCategoriaQuery : IRequest<CategoriaDto>
    {
        [Required(ErrorMessage = "Informe o Id da categoria")]
        public required Guid CategoriaId { get; set; }
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

            if (conta == null)
                return null;

            var contaDto = new ContaDetalheDto
            {
                Id = conta.Id,
                Nome = conta.Nome,
                Tipo = conta.Tipo,
                Saldo = conta.Saldo,
                Ativo = conta.Ativo,
                LimiteDisponivel = conta.EhCartaoCredito() ? (conta.Limite ?? 0) - conta.Saldo : null,
                QuantidadeTransacoes = 0, // mock
                TotalReceitas = 0,        // mock
                TotalDespesas = 0         // mock
            };

            return contaDto;
        }
    }
}
