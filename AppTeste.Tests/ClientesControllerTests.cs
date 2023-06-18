using AppTeste.Controllers;
using AppTeste.Models.DTOs;
using AppTeste.Models.Entities;
using AppTeste.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTeste.Tests
{
    [TestClass]
    public class ClientesControllerTests
    {
        private ClientesController _controller;
        private Mock<IValidatorFactory> _validatorFactoryMock;
        private Mock<IClientesRepository> _clientesRepositoryMock;

        [TestInitialize]
        public void Setup()
        {
            _validatorFactoryMock = new Mock<IValidatorFactory>();
            _clientesRepositoryMock = new Mock<IClientesRepository>();
            _controller = new ClientesController(_validatorFactoryMock.Object, _clientesRepositoryMock.Object);
        }

        [TestMethod]
        public async Task ListarTodos_WithValidInput_ShouldReturnOkWithClientes()
        {
            // Arrange
            var inputModel = new ListarTodosClientesInputModel();

            var expectedClientes = new List<Cliente>() { };
            _clientesRepositoryMock.Setup(cr => cr.ListarClientes(inputModel)).ReturnsAsync(expectedClientes);

            // Act
            var result = await _controller.ListarTodos(inputModel) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual(expectedClientes, result.Value);
        }

        [TestMethod]
        public async Task ListarTodos_WithException_ShouldReturnInternalServerError()
        {
            // Arrange
            var inputModel = new ListarTodosClientesInputModel();

            _clientesRepositoryMock.Setup(cr => cr.ListarClientes(inputModel)).Throws(new Exception());

            // Act
            var result = await _controller.ListarTodos(inputModel) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

        //----------------
        [TestMethod]
        public async Task Buscar_WithValidId_ShouldReturnOkWithCliente()
        {
            // Arrange
            int idCliente = 1;
            var cliente = new Cliente { ID = idCliente, Nome = "Pedro" };
            _clientesRepositoryMock.Setup(r => r.ObterClientePorId(idCliente)).ReturnsAsync(cliente);

            // Act
            var result = await _controller.Buscar(idCliente);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.AreEqual(cliente, okResult.Value);
        }

        [TestMethod]
        public async Task Buscar_WithNegativeId_ShouldReturnBadRequest()
        {
            // Arrange
            int idCliente = -1;

            // Act
            var result = await _controller.Buscar(idCliente);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.AreEqual($"Campo: idCliente é obrigatório!", badRequestResult.Value);
        }

        [TestMethod]
        public async Task Buscar_WithNonExistingId_ShouldReturnNotFound()
        {
            // Arrange
            int idCliente = 1;
            Cliente cliente = null;
            _clientesRepositoryMock.Setup(r => r.ObterClientePorId(idCliente)).ReturnsAsync(cliente);

            // Act
            var result = await _controller.Buscar(idCliente);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.AreEqual("Nenhum cliente encontrado para esse idCliente", notFoundResult.Value);
        }

        [TestMethod]
        public async Task Buscar_ExceptionThrown_ShouldReturnInternalServerError()
        {
            // Arrange
            int idCliente = 1;
            _clientesRepositoryMock.Setup(r => r.ObterClientePorId(idCliente)).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.Buscar(idCliente);

            // Assert
            var problemResult = result as ObjectResult;
            Assert.IsNotNull(problemResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, problemResult.StatusCode.Value);
        }

        //---------------------

        [TestMethod]
        public async Task Inserir_WithValidInput_ShouldReturnOkWithTrue()
        {
            // Arrange
            var inserirClienteInputModel = new InserirClienteInputModel
            {
                Nome = "Pedro",
                RG = "123456",
                CPF = "123456789",
                DataNascimento = DateTime.Now,
                Telefone = "1234567890",
                Email = "pedro@example.com",
                CodEmpresa = Enums.EnumCodEmpresa.Atacadao
            };

            _clientesRepositoryMock.Setup(r => r.RealizarCadastroCliente(inserirClienteInputModel)).ReturnsAsync(true);

            // Act
            var result = await _controller.Inserir(inserirClienteInputModel);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode.Value);
            Assert.AreEqual(true, okResult.Value);
        }


        [TestMethod]
        public async Task Inserir_ExceptionThrown_ShouldReturnInternalServerError()
        {
            // Arrange
            var inserirClienteInputModel = new InserirClienteInputModel();
            _clientesRepositoryMock.Setup(r => r.RealizarCadastroCliente(inserirClienteInputModel)).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.Inserir(inserirClienteInputModel);

            // Assert
            var problemResult = result as ObjectResult;
            Assert.IsNotNull(problemResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, problemResult.StatusCode.Value);
        }

        //-------------------

        [TestMethod]
        public async Task Atualizar_WithValidInput_ShouldReturnOkWithTrue()
        {
            // Arrange
            var alterarClienteInputModel = new AlterarClienteInputModel
            {
                Id = 1,
                Nome = "Pedro",
                RG = "123456",
                CPF = "123456789",
                DataNascimento = DateTime.Now,
                Telefone = "1234567890",
                Email = "pedro@example.com",
                CodEmpresa = Enums.EnumCodEmpresa.Atacadao
            };

            _clientesRepositoryMock.Setup(r => r.RealizarAlteracaoCliente(alterarClienteInputModel)).ReturnsAsync(true);

            // Act
            var result = await _controller.Atualizar(alterarClienteInputModel);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode.Value);
            Assert.AreEqual(true, okResult.Value);
        }

        [TestMethod]
        public async Task Atualizar_ExceptionThrown_ShouldReturnInternalServerError()
        {
            // Arrange
            var alterarClienteInputModel = new AlterarClienteInputModel();
            _clientesRepositoryMock.Setup(r => r.RealizarAlteracaoCliente(alterarClienteInputModel)).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.Atualizar(alterarClienteInputModel);

            // Assert
            var problemResult = result as ObjectResult;
            Assert.IsNotNull(problemResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, problemResult.StatusCode.Value);
        }

        //---------------------------

        [TestMethod]
        public async Task Remover_WithValidInput_ShouldReturnOkWithTrue()
        {
            // Arrange
            var idCliente = 1;
            _clientesRepositoryMock.Setup(r => r.RealizarExclusaoCliente(idCliente)).ReturnsAsync(true);

            // Act
            var result = await _controller.Remover(idCliente);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode.Value);
            Assert.AreEqual(true, okResult.Value);
        }

        [TestMethod]
        public async Task Remover_ExceptionThrown_ShouldReturnInternalServerError()
        {
            // Arrange
            var idCliente = 1;
            _clientesRepositoryMock.Setup(r => r.RealizarExclusaoCliente(idCliente)).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.Remover(idCliente);

            // Assert
            var problemResult = result as ObjectResult;
            Assert.IsNotNull(problemResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, problemResult.StatusCode.Value);
        }
    }
}