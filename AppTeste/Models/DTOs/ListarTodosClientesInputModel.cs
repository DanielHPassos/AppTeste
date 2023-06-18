using AppTeste.Enums;

namespace AppTeste.Models.DTOs
{
    public class ListarTodosClientesInputModel
    {
        public EnumCodEmpresa? CodEmpresa { get; set; }
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public string? Cidade { get; set; }
        public string? Estado { get; set; }

    }
}
