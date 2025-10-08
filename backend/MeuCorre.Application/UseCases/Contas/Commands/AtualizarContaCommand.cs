using MediatR;
using MeuCorre.Domain.Entities;
using MeuCorre.Domain.Enums;
using MeuCorre.Domain.Interfaces.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace MeuCorre.Application.UseCases.Contas.Commands
{
    public class AtualizarContaCommand : IRequest<(string, bool)>
    {
        [Required(ErrorMessage = "Id da conta é obrigatório")]
        public required Guid ContaId { get; set; }

        [Required(ErrorMessage = "Id do usuário é obrigatório")]
        public required Guid UsuarioId { get; set; }

        [Required(ErrorMessage = "Nome da conta é obrigatório")]
        public required string Nome { get; set; }

        public bool? Ativo { get; set; }
        public decimal? Limite { get; set; }          // apenas para cartões
        public int? DiaFechamento { get; set; }       // apenas para cartões
        public int? DiaVencimento { get; set; }       // apenas para cartões
        public string? Cor { get; set; }
        public string? Icone { get; set; }
    }

    internal class AtualizarContaCommandHandler : IRequestHandler<AtualizarContaCommand, (string, bool)>
    {
        private readonly IContaRepository<Conta> _contaRepository;

        public AtualizarContaCommandHandler(IContaRepository<Conta> contaRepository)
        {
            _contaRepository = contaRepository;
        }

        public async Task<(string, bool)> Handle(AtualizarContaCommand request, CancellationToken cancellationToken)
        {
            // Buscar conta pelo Id e UsuarioId
            var conta = await _contaRepository.ObterPorIdEUsuarioAsync(request.ContaId, request.UsuarioId);

            if (conta == null)
                return ("Conta não encontrada ou não pertence ao usuário", false);

            // NÃO alterar Tipo ou Saldo
            // Atualizar apenas os campos permitidos
            conta.Nome = request.Nome;
            if (request.Ativo.HasValue)
                conta.Ativo = request.Ativo.Value;
            if (request.Limite.HasValue)
                conta.Limite = request.Limite.Value;
            if (request.DiaFechamento.HasValue)
                conta.DiaFechamento = request.DiaFechamento.Value;
            if (request.DiaVencimento.HasValue)
                conta.DiaVencimento = request.DiaVencimento.Value;
            if (!string.IsNullOrWhiteSpace(request.Cor))
                conta.Cor = request.Cor;
            if (!string.IsNullOrWhiteSpace(request.Icone))
                conta.Icone = request.Icone;

            conta.DataAtualizacao = DateTime.UtcNow;

            await _contaRepository.AtualizarAsync(conta);

            return ("Conta atualizada com sucesso", true);
        }
    }
}
