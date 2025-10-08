using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MeuCorre.Domain.Entities;
using MeuCorre.Domain.Interfaces.Repositories;

namespace MeuCorre.Application.UseCases.Contas.Commands
{
    public class ExcluirContaCommand : IRequest<(string, bool)>
    {
        [Required(ErrorMessage = "É necessário informar o ID da conta")]
        public required Guid ContaId { get; set; }

        [Required(ErrorMessage = "É necessário informar o ID do usuário")]
        public required Guid UsuarioId { get; set; }

        [Required(ErrorMessage = "É necessário confirmar a exclusão")]
        public required bool Confirmar { get; set; }
    }

    internal class ExcluirContaCommandHandler : IRequestHandler<ExcluirContaCommand, (string, bool)>
    {
        private readonly IContaRepository<Conta> _contaRepository;

        public ExcluirContaCommandHandler(IContaRepository<Conta> contaRepository)
        {
            _contaRepository = contaRepository;
        }

        public async Task<(string, bool)> Handle(ExcluirContaCommand request, CancellationToken cancellationToken)
        {
            if (!request.Confirmar)
                return ("Confirmação de exclusão é necessária", false);

            var conta = await _contaRepository.ObterPorIdEUsuarioAsync(request.ContaId, request.UsuarioId);

            if (conta == null)
                return ("Conta não encontrada ou não pertence ao usuário", false);

            // Validar saldo zero
            if (conta.Saldo != 0)
                return ("Não é possível excluir a conta: saldo deve ser zero", false);

            // OBS: validação de transações futuras pode ser adicionada depois

            // Excluir permanentemente do banco usando o método da interface
            await _contaRepository.RemoverAsync(conta);

            return ("Conta excluída com sucesso", true);
        }
    }
}
