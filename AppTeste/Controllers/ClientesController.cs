using AppTeste.Models.DTOs;
using AppTeste.Models.Entities;
using AppTeste.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppTeste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IValidatorFactory _validatorFactory;
        private readonly IClientesRepository clientesRepository;
        public ClientesController(IValidatorFactory validatorFactory, IClientesRepository clientesRepository)
        {
            this._validatorFactory = validatorFactory;
            this.clientesRepository = clientesRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Cliente>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> ListarTodos([FromQuery] ListarTodosClientesInputModel listarTodosClientesInputModel)
        {
            try
            {

                var clientes = await clientesRepository.ListarClientes(listarTodosClientesInputModel);

                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return Problem("Ocorreu um erro interno do servidor.", statusCode: 500);
            }
        }

        [HttpGet("{idCliente}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Cliente))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> Buscar([FromRoute] int idCliente)
        {
            try
            {
                if (idCliente < 0)
                    return BadRequest($"Campo: {nameof(idCliente)} é obrigatório!");

                var cliente = await clientesRepository.ObterClientePorId(idCliente);

                if (cliente == null)
                    return NotFound("Nenhum cliente encontrado para esse idCliente");

                return Ok(cliente);

            }
            catch (Exception ex)
            {
                return Problem("Ocorreu um erro interno do servidor.", statusCode: 500);
            }
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> Inserir([FromBody] InserirClienteInputModel inserirClienteInputModel)
        {
            try
            {
   
                var sucesso = await clientesRepository.RealizarCadastroCliente(inserirClienteInputModel);

                return Ok(sucesso);

            }
            catch (Exception ex)
            {
                return Problem("Ocorreu um erro interno do servidor.", statusCode: 500);
            }
        }

        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> Atualizar([FromBody] AlterarClienteInputModel alterarClienteInputModel)
        {
            try
            {
         
                var sucesso = await clientesRepository.RealizarAlteracaoCliente(alterarClienteInputModel);

                return Ok(sucesso);

            }
            catch (Exception ex)
            {
                return Problem("Ocorreu um erro interno do servidor.", statusCode: 500);
            }
        }

        [HttpDelete("{idCliente}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> Remover([FromRoute] int idCliente)
        {
            try
            {
                if (idCliente < 0)
                    return BadRequest($"Campo: {nameof(idCliente)} é obrigatório!");

                var sucesso = await clientesRepository.RealizarExclusaoCliente(idCliente);

                return Ok(sucesso);
            }
            catch (Exception ex)
            {
                return Problem("Ocorreu um erro interno do servidor.", statusCode: 500);
            }
        }
    }
}
