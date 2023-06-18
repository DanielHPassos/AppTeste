using AppTeste.Enums;
using System.ComponentModel.DataAnnotations;

namespace AppTeste.Models.Entities
{
    public class Cliente
    {
        [Key]
        public int ID { get; set; }
        public string Nome { get; set; }
        public string RG { get; set; }
        public string CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public EnumCodEmpresa CodEmpresa { get; set; }
        public ICollection<ClienteEndereco> ClienteEnderecos { get; set; }


    }
}
