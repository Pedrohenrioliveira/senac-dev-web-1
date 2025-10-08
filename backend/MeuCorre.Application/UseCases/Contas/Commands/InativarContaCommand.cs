using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MeuCorre.Domain.Entities;
using MeuCorre.Domain.Interfaces.Repositories;

namespace MeuCorre.Application.UseCases.Contas.Commands
{
    public class InativarContaCommand : IRequest<(string, bool)>
    {
        [Required(ErrorMessage = "É necessário informar o ID da conta")]
        public required Guid ContaId { get; set; }

        [Required(ErrorMessage = "É necessário informar o ID do usuário")]
        public required Guid UsuarioId { get; set; }
    }

    internal class InativarContaCommandHandler : IRequestHandler<InativarContaCommand, (string, bool)>
    {
        private readonly IContaRepository<Conta> _contaRepository;

        public InativarContaCommandHandler(IContaRepository<Conta> contaRepository)
        {
            _contaRepository = contaRepository;
        }

        public async Task<(string, bool)> Handle(InativarContaCommand request, CancellationToken cancellationToken)
        {
            // Buscar conta pelo Id e UsuarioId
            var conta = await _contaRepository.ObterPorIdEUsuarioAsync(request.ContaId, request.UsuarioId);

            if (conta == null)
                return ("Conta não encontrada ou não pertence ao usuário", false);

            // Validar saldo zero
            if (conta.Saldo != 0)
                return ("Não é possível inativar a conta: saldo deve ser zero", false);

            // OBS: validação de transações futuras pode ser adicionada depois

            // Marcar como inativa
            conta.Ativo = false;
            conta.DataAtualizacao = DateTime.UtcNow;

            await _contaRepository.AtualizarAsync(conta);

            return ("Conta inativada com sucesso", true);
        }
    }
}
