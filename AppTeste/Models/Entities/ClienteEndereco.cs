namespace AppTeste.Models.Entities
{
    public class ClienteEndereco
    {
        public int ID { get; set; }

        public int ClienteID { get; set; }
        public int EnderecoID { get; set; }

        public Cliente Cliente { get; set; }
        public Endereco Endereco { get; set; }
    }
}
