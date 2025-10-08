using MediatR;
using MeuCorre.Domain.Entities;
using MeuCorre.Domain.Enums;
using MeuCorre.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeuCorre.Infra;
using MeuCorre.Infra.Data.Context;

namespace MeuCorre.Application.UseCases.Contas.Commands
{
    public record CriarContaResponse(
        Guid Id,
        Guid UsuarioId,
        string Nome,
        TipoConta Tipo,
        decimal Saldo,
        decimal? Limite,
        int? DiaFechamento,
        int? DiaVencimento,
        string? Cor,
        string? Icone,
        string? TipoLimite,
        bool Ativo,
        DateTime DataCriacao,
        bool Sucesso,
        string Mensagem
    );

    public class CriarContaCommand : IRequest<CriarContaResponse>
    {
        [Required(ErrorMessage = "É necessário informar o id do usuário")]
        public required Guid UsuarioId { get; set; }

        [Required(ErrorMessage = "Nome da conta é obrigatório")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "Tipo da conta é obrigatório")]
        public required TipoConta Tipo { get; set; }

        public decimal Saldo { get; set; } = 0;
        public decimal? Limite { get; set; }
        public int? DiaFechamento { get; set; }
        public int? DiaVencimento { get; set; }
        public string? Cor { get; set; }
        public string? Icone { get; set; }
        public string? TipoLimite { get; set; }
    }

    internal class CriarContaCommandHandler : IRequestHandler<CriarContaCommand, CriarContaResponse>
    {
        private readonly IContaRepository<Conta> _contaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly MeuDbContext _dbContext;

        public CriarContaCommandHandler(
            IContaRepository<Conta> contaRepository,
            IUsuarioRepository usuarioRepository,
            MeuDbContext dbContext)
        {
            _contaRepository = contaRepository;
            _usuarioRepository = usuarioRepository;
            _dbContext = dbContext;
        }

        public async Task<CriarContaResponse> Handle(CriarContaCommand request, CancellationToken cancellationToken)
        {
        
            var usuario = await _usuarioRepository.ObterUsuarioPorId(request.UsuarioId);
            if (usuario == null)
            {
                return new CriarContaResponse(Guid.Empty, request.UsuarioId, request.Nome, request.Tipo, request.Saldo,
                    request.Limite, request.DiaFechamento, request.DiaVencimento, request.Cor, request.Icone, request.TipoLimite,
                    false, DateTime.MinValue, false, "Usuário inválido");
            }

         
            var existe = await _contaRepository.ExisteContaComNomeAsync(request.UsuarioId, request.Nome);
            if (existe)
            {
                return new CriarContaResponse(Guid.Empty, request.UsuarioId, request.Nome, request.Tipo, request.Saldo,
                    request.Limite, request.DiaFechamento, request.DiaVencimento, request.Cor, request.Icone, request.TipoLimite,
                    false, DateTime.MinValue, false, "Já existe uma conta com este nome para o usuário");
            }

    
            if (request.Saldo < 0)
            {
                request.Saldo *= -1;
            }

            if (request.Tipo == TipoConta.CartaoCredito && !request.DiaFechamento.HasValue && request.DiaVencimento.HasValue)
            {
                int calculado = request.DiaVencimento.Value - 10;
                if (calculado < 1) calculado = 1;
                if (calculado > 31) calculado = 31;
                request.DiaFechamento = calculado;
            }

            var now = DateTime.UtcNow;
            var conta = new Conta
            {
                UsuarioId = request.UsuarioId,
                Nome = request.Nome,
                Tipo = request.Tipo.ToString(), 
                Saldo = request.Saldo,
                Limite = request.Limite,
                DiaFechamento = request.DiaFechamento,
                DiaVencimento = request.DiaVencimento,
                Cor = request.Cor ?? string.Empty,
                Icone = request.Icone ?? string.Empty,
                TipoLimite = request.TipoLimite,
                Ativo = true,
                DataCriacao = now
            };

            _dbContext.Contas.Add(conta);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CriarContaResponse(
                conta.Id,
                conta.UsuarioId,
                conta.Nome,
                request.Tipo,
                conta.Saldo,
                conta.Limite,
                conta.DiaFechamento,
                conta.DiaVencimento,
                conta.Cor,
                conta.Icone,
                conta.TipoLimite,
                conta.Ativo,
                conta.DataCriacao,
                true,
                "Conta criada com sucesso"
            );
        }
    }
}
