using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MeuCorre.Application.UseCases.Contas.Commands;
using MeuCorre.Application.UseCases.Contas.Queries;
using MeuCorre.Domain.Enums;

namespace MeuCorre.API.Controllers
{
    [ApiController]
    [Route("api/v1/contas")]
    public class ContasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Cria uma nova conta
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CriarConta([FromBody] CriarContaCommand command)
        {
            var response = await _mediator.Send(command);

            if (response.Sucesso)
                return CreatedAtAction(nameof(ObterConta), new { id = response.Id }, response.Mensagem);
            else
                return Conflict(response.Mensagem);
        }

        /// <summary>
        /// Lista contas do usuário
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ListarContas([FromQuery] TipoConta? tipo, [FromQuery] bool apenasAtivas = true, [FromQuery] string ordenarPor = "Nome")
        {
            var query = new ListarContasQuery
            {              
                ApenasAtivas = apenasAtivas,
                OrdenarPor = ordenarPor
            };

            var contas = await _mediator.Send(query);
            return Ok(contas);
        }

        /// <summary>
        /// Obter conta por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterConta(Guid id, [FromQuery] Guid usuarioId)
        {
            var query = new ObterContaQuery
            {
                ContaId = id,
                UsuarioId = usuarioId
            };

            var conta = await _mediator.Send(query);
            if (conta == null)
                return NotFound("Conta não encontrada");

            return Ok(conta);
        }

        /// <summary>
        /// Atualiza uma conta existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarConta(Guid id, [FromBody] AtualizarContaCommand command)
        {
            command.ContaId = id;
            var (mensagem, sucesso) = await _mediator.Send(command);

            if (sucesso)
                return Ok(mensagem);
            else
                return NotFound(mensagem);
        }

        /// <summary>
        /// Inativar uma conta
        /// </summary>
        [HttpPatch("{id}/inativar")]
        public async Task<IActionResult> InativarConta(Guid id, [FromBody] InativarContaCommand command)
        {
            command.ContaId = id;
            var (mensagem, sucesso) = await _mediator.Send(command);

            if (sucesso)
                return NoContent();
            else
                return Conflict(mensagem);
        }

        /// <summary>
        /// Reativar uma conta
        /// </summary>
        [HttpPatch("{id}/reativar")]
        public async Task<IActionResult> ReativarConta(Guid id, [FromBody] ReativarContaCommand command)
        {
            command.ContaId = id;
            var (mensagem, sucesso) = await _mediator.Send(command);

            if (sucesso)
                return NoContent();
            else
                return Conflict(mensagem);
        }

        /// <summary>
        /// Excluir permanentemente uma conta
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirConta(Guid id, [FromQuery] Guid usuarioId, [FromQuery] bool confirmar)
        {
            var command = new ExcluirContaCommand
            {
                ContaId = id,
                UsuarioId = usuarioId,
                Confirmar = confirmar
            };

            var (mensagem, sucesso) = await _mediator.Send(command);

            if (sucesso)
                return NoContent();
            else
                return Conflict(mensagem);
        }

        /// <summary>
        /// Obter saldo consolidado de todas as contas do usuário
        /// </summary>
        [HttpGet("saldo-consolidado")]
        public async Task<IActionResult> SaldoConsolidado([FromQuery] Guid usuarioId)
        {
            var query = new ObterSaldoConsolidadoQuery
            {
                UsuarioId = usuarioId
            };

            var saldo = await _mediator.Send(query);
            return Ok(saldo);
        }
    }
}
