namespace AppTeste.Models.Entities
{
    public class Cidade
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Estado { get; set; }

        public ICollection<Endereco> Enderecos { get; set; }
    }
}
