using MeuCorre.Domain.Entities;
using MeuCorre.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeuCorre.Domain.Interfaces.Repositories
{
    public interface IContaRepository<Conta>
    {
        Task<List<Conta>> ObterPorUsuarioAsync(Guid usuarioId, bool apenasAtivas = true);
        Task<List<Conta>> ObterPorTipoAsync(Guid usuarioId, TipoConta tipo);
        Task<Conta?> ObterPorIdEUsuarioAsync(Guid contaId, Guid usuarioId);
        Task<bool> ExisteContaComNomeAsync(Guid usuarioId, string nome, Guid? contaIdExcluir = null);
        Task<decimal> CalcularSaldoTotalAsync(Guid usuarioId);

        Task CriarContaAsync(Conta conta);
        Task AtualizarAsync(Conta conta);
        Task RemoverAsync(Conta conta); // <- necessário para excluir permanentemente
    }
}
