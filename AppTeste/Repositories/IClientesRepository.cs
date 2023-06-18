using AppTeste.Models.DTOs;
using AppTeste.Models.Entities;

namespace AppTeste.Repositories
{
    public interface IClientesRepository
    {
        Task<IEnumerable<Cliente>> ListarClientes(ListarTodosClientesInputModel filtro);
        Task<Cliente> ObterClientePorId(int clienteId);
        Task<bool> RealizarAlteracaoCliente(AlterarClienteInputModel alterarClienteInputModel);
        Task<bool> RealizarExclusaoCliente(int idCliente);
        Task<bool> RealizarCadastroCliente(InserirClienteInputModel inputModel);
    }
}
