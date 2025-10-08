using MeuCorre.Domain.Entities;
using MeuCorre.Domain.Enums;
using MeuCorre.Domain.Interfaces.Repositories;
using MeuCorre.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeuCorre.Infra.Repositories
{
    public class ContaRepository : IContaRepository<Conta>
    {
        private readonly MeuDbContext _meuDbContext;

        public ContaRepository(MeuDbContext meuDbContext)
        {
            _meuDbContext = meuDbContext;
        }

        public async Task<List<Conta>> ObterPorUsuarioAsync(Guid usuarioId, bool apenasAtivas = true)
        {
            var query = _meuDbContext.Contas
                .Include(c => c.Usuario)
                .Where(c => c.UsuarioId == usuarioId);

            if (apenasAtivas)
                query = query.Where(c => c.Ativo);

            return await query
                .OrderBy(c => c.Nome)
                .ToListAsync();
        }

        public async Task<List<Conta>> ObterPorTipoAsync(Guid usuarioId, TipoConta tipo)
        {
            return await _meuDbContext.Contas
                .Where(c => c.UsuarioId == usuarioId && c.Tipo == tipo.ToString())
                .ToListAsync();
        }

        public async Task<Conta?> ObterPorIdEUsuarioAsync(Guid contaId, Guid usuarioId)
        {
            return await _meuDbContext.Contas
                .FirstOrDefaultAsync(c => c.Id == contaId && c.UsuarioId == usuarioId);
        }

        public async Task<bool> ExisteContaComNomeAsync(Guid usuarioId, string nome, Guid? contaIdExcluir = null)
        {
            var query = _meuDbContext.Contas
                .Where(c => c.UsuarioId == usuarioId && c.Nome.ToLower() == nome.ToLower());

            if (contaIdExcluir.HasValue)
                query = query.Where(c => c.Id != contaIdExcluir.Value);

            return await query.AnyAsync();
        }

        public async Task<decimal> CalcularSaldoTotalAsync(Guid usuarioId)
        {
            return await _meuDbContext.Contas
                .Where(c => c.UsuarioId == usuarioId && c.Ativo)
                .SumAsync(c => c.Saldo);
        }

        public async Task CriarContaAsync(Conta conta)
        {
            await _meuDbContext.Contas.AddAsync(conta);
            await _meuDbContext.SaveChangesAsync();
        }

        // CORRIGIDO: nome do método agora bate com a interface
        public async Task AtualizarAsync(Conta conta)
        {
            _meuDbContext.Contas.Update(conta);
            await _meuDbContext.SaveChangesAsync();
        }

        public async Task RemoverContaAsync(Conta conta)
        {
            _meuDbContext.Contas.Remove(conta);
            await _meuDbContext.SaveChangesAsync();
        }
        public async Task RemoverAsync(Conta conta)
        {
            _meuDbContext.Contas.Remove(conta);
            await _meuDbContext.SaveChangesAsync();
        }
    }
}
