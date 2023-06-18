using AppTeste.Enums;

namespace AppTeste.Models.DTOs
{
    public class InserirClienteInputModel
    {
        public string Nome { get; set; }
        public string RG { get; set; }
        public string CPF { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public EnumCodEmpresa? CodEmpresa { get; set; }

        public string Rua { get; set; }
        public string Bairro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string CEP { get; set; }
        public EnumTipoEndereco? TipoEndereco { get; set; }

        public string Cidade { get; set; }
        public string Estado { get; set; }
    }
}
