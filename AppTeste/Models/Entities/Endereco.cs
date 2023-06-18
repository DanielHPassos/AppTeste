using AppTeste.Enums;
using System.ComponentModel.DataAnnotations;

namespace AppTeste.Models.Entities
{
    public class Endereco
    {
        [Key]
        public int ID { get; set; }
        public string Rua { get; set; }
        public string Bairro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string CEP { get; set; }
        public EnumTipoEndereco TipoEndereco { get; set; }
        public int CidadeId { get; set; }

        public Cidade Cidade { get; set; }
        public ICollection<ClienteEndereco> ClienteEnderecos { get; set; }

    }
}
