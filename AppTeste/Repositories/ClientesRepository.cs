using AppTeste.Enums;
using AppTeste.Models;
using AppTeste.Models.DTOs;
using AppTeste.Models.Entities;
using AppTeste.Repositories.Context;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AppTeste.Repositories
{
    public class ClientesRepository : DbContext, IClientesRepository
    {
        private readonly ConfigSettings configuration;
        private readonly AppTesteDbContext dbContext;
       
        public ClientesRepository(ConfigSettings configuration, AppTesteDbContext dbContext)
        {
            this.configuration = configuration;
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<Cliente>> ListarClientes(ListarTodosClientesInputModel filtro)
        {
            using (var connection = new SqlConnection(configuration.ConnectionString))
            {
                await connection.OpenAsync();

                var query = @"
                SELECT c.ID, c.Nome, c.RG, c.CPF, c.Data_Nascimento as DataNascimento, c.Telefone, c.Email, c.COD_EMPRESA as CodEmpresa
                FROM TB_CLIENTE c
                LEFT JOIN TB_CLIENTE_ENDERECO ce ON c.ID = ce.TB_CLIENTE_ID
                LEFT JOIN TB_ENDERECO e ON ce.TB_ENDERECO_ID = e.ID
                LEFT JOIN TB_CIDADE cid ON e.TB_CIDADE_ID = cid.ID
                WHERE (@CodEmpresa IS NULL OR c.COD_EMPRESA = @CodEmpresa)
                  AND (@Nome IS NULL OR c.Nome LIKE '%' + @Nome + '%')
                  AND (@CPF IS NULL OR c.CPF = @CPF)
                  AND (@Cidade IS NULL OR cid.Nome LIKE '%' + @Cidade + '%')
                  AND (@Estado IS NULL OR cid.Estado = @Estado)";

                var parametros = new
                {
                    CodEmpresa = filtro.CodEmpresa,
                    Nome = filtro.Nome,
                    CPF = filtro.Cpf,
                    Cidade = filtro.Cidade,
                    Estado = filtro.Estado
                };

                return await connection.QueryAsync<Cliente>(query, parametros);
            }
        }

        public async Task<Cliente> ObterClientePorId(int clienteId)
        {
            var cliente = await this.dbContext.Set<Cliente>()
                .FirstOrDefaultAsync(c => c.ID == clienteId);

            return cliente;
        }

        public async Task<bool> RealizarAlteracaoCliente(AlterarClienteInputModel alterarClienteInputModel)
        {
            var cpfExists = await dbContext.Clientes.AnyAsync(c => c.CPF == alterarClienteInputModel.CPF);
            if (!cpfExists)
                return false;

            var cliente = await dbContext.Clientes.Include(c => c.ClienteEnderecos).ThenInclude(x => x.Endereco).ThenInclude(x => x.Cidade)
                                         .SingleOrDefaultAsync(c => c.ID == alterarClienteInputModel.Id);

            if (cliente != null)
            {
                cliente.Nome = alterarClienteInputModel.Nome;
                cliente.RG = alterarClienteInputModel.RG;
                cliente.DataNascimento = alterarClienteInputModel.DataNascimento.Value;
                cliente.Telefone = alterarClienteInputModel.Telefone;
                cliente.Email = alterarClienteInputModel.Email;

                var enderecoCliente = cliente.ClienteEnderecos.FirstOrDefault();
                if (enderecoCliente != null)
                {
                    var endereco = enderecoCliente.Endereco;
                    endereco.Rua = alterarClienteInputModel.Rua;
                    endereco.Bairro = alterarClienteInputModel.Bairro;
                    endereco.Numero = alterarClienteInputModel.Numero;
                    endereco.Complemento = alterarClienteInputModel.Complemento;
                    endereco.CEP = alterarClienteInputModel.CEP;

                    var cidade = endereco.Cidade;
                    if (cidade != null)
                    {
                        cidade.Nome = alterarClienteInputModel.Cidade;
                        cidade.Estado = alterarClienteInputModel.Estado;
                    }
                }

                await dbContext.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> RealizarExclusaoCliente(int idCliente)
        {
            var cliente = await dbContext.Clientes.Include(c => c.ClienteEnderecos)
                                        .SingleOrDefaultAsync(c => c.ID == idCliente);

            if (cliente != null)
            {
                dbContext.Clientes.Remove(cliente);
                await dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> RealizarCadastroCliente(InserirClienteInputModel inputModel)
        {
            bool cpfExists = await VerificarCpfExistenteParaEmpresa(inputModel.CPF, inputModel.CodEmpresa.Value);

            if (cpfExists)
            {
                var clientes = await ObterClientePorCpf(inputModel.CPF, inputModel.CodEmpresa.Value);

                if (!clientes.Any(c => c.ClienteEnderecos.Any(x => x.Endereco.TipoEndereco == inputModel.TipoEndereco.Value)))
                {
                    await RealizarCadastro(inputModel);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                await RealizarCadastro(inputModel);
            }

            return true;
        }

        private async Task<bool> VerificarCpfExistenteParaEmpresa(string cpf, EnumCodEmpresa codEmpresa)
        {
            return await dbContext.Clientes.AnyAsync(c => c.CPF == cpf && c.CodEmpresa == codEmpresa);
        }

        private async Task<IEnumerable<Cliente>> ObterClientePorCpf(string cpf, EnumCodEmpresa codEmpresa)
        {
            return await dbContext.Clientes.Include(x => x.ClienteEnderecos).ThenInclude(x => x.Endereco).Where(c => c.CPF == cpf && c.CodEmpresa == codEmpresa).ToListAsync();
        }

        private async Task RealizarCadastro(InserirClienteInputModel inputModel)
        {
            var cliente = new Cliente
            {
                Nome = inputModel.Nome,
                RG = inputModel.RG,
                CPF = inputModel.CPF,
                DataNascimento = inputModel.DataNascimento.Value,
                Telefone = inputModel.Telefone,
                Email = inputModel.Email,
                CodEmpresa = inputModel.CodEmpresa.Value
            };

            var cidade = new Cidade
            {
                Nome = inputModel.Cidade,
                Estado = inputModel.Estado
            };

            var endereco = new Endereco
            {
                Rua = inputModel.Rua,
                Bairro = inputModel.Bairro,
                Numero = inputModel.Numero,
                Complemento = inputModel.Complemento,
                CEP = inputModel.CEP,
                TipoEndereco = inputModel.TipoEndereco.Value,
                Cidade = cidade
            };

            var clienteEndereco = new ClienteEndereco
            {
                Cliente = cliente,
                Endereco = endereco
            };

            dbContext.Clientes.Add(cliente);
            dbContext.Cidades.Add(cidade);
            dbContext.Enderecos.Add(endereco);
            dbContext.ClientesEnderecos.Add(clienteEndereco);

            await dbContext.SaveChangesAsync();
        }
    }
}
