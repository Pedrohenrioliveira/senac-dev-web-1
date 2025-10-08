using MediatR;
using MeuCorre.Domain.Entities;
using MeuCorre.Domain.Enums;
using MeuCorre.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeuCorre.Application.UseCases.Contas.Responses;

namespace MeuCorre.Application.UseCases.Contas.Queries
{
    // Query → define os parâmetros de entrada
    public class ListarContasQuery : IRequest<List<ContaResumoResponse>>
    {
        public Guid UsuarioId { get; set; }
        public TipoConta? FiltrarPorTipo { get; set; }
        public bool ApenasAtivas { get; set; } = true;
        public string OrdenarPor { get; set; } = "Nome"; // Nome, Saldo, Tipo, etc.
    }

    // Handler → processa a Query
    public class ListarContasQueryHandler : IRequestHandler<ListarContasQuery, List<ContaResumoResponse>>
    {
        private readonly IContaRepository<Conta> _contaRepository;

        public ListarContasQueryHandler(IContaRepository<Conta> contaRepository)
        {
            _contaRepository = contaRepository;
        }

        public async Task<List<ContaResumoResponse>> Handle(ListarContasQuery request, CancellationToken cancellationToken)
        {
            // Buscar contas do usuário
            var contas = await _contaRepository.ObterPorUsuarioAsync(request.UsuarioId, request.ApenasAtivas);

            // Aplicar filtro por tipo, se houver
            if (request.FiltrarPorTipo.HasValue)
            {
                contas = contas.Where(c => c.Tipo == request.FiltrarPorTipo.Value.ToString()).ToList();
            }

            // Mapear contas para ContaResumoResponse
            var contasResumo = contas.Select(c => new ContaResumoResponse
            {
                Id = c.Id,
                Nome = c.Nome,
                Tipo = c.Tipo.ToString(),
                Saldo = c.Saldo,
                Ativo = c.Ativo,
                LimiteDisponivel = c.EhCartaoCredito() ? (c.Limite ?? 0) - c.Saldo : null
            }).ToList();

            // Ordenar conforme solicitado
            contasResumo = request.OrdenarPor.ToLower() switch
            {
                "saldo" => contasResumo.OrderByDescending(c => c.Saldo).ToList(),
                "tipo" => contasResumo.OrderBy(c => c.Tipo).ToList(),
                _ => contasResumo.OrderBy(c => c.Nome).ToList(),
            };

            return contasResumo;
        }
    }
}