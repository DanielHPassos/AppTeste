using AppTeste.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppTeste.Repositories.Context
{
    public class AppTesteDbContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<ClienteEndereco> ClientesEnderecos { get; set; }
        public DbSet<Cidade> Cidades { get; set; }

        public AppTesteDbContext(DbContextOptions<AppTesteDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("TB_CLIENTE");
                entity.HasKey(c => c.ID);

                entity.Property(x => x.ID).HasColumnName("ID").ValueGeneratedOnAdd();
                entity.Property(x => x.Nome).HasColumnName("NOME");
                entity.Property(x => x.RG).HasColumnName("RG");
                entity.Property(x => x.CPF).HasColumnName("CPF");
                entity.Property(x => x.DataNascimento).HasColumnName("DATA_NASCIMENTO");
                entity.Property(x => x.Telefone).HasColumnName("TELEFONE");
                entity.Property(x => x.Email).HasColumnName("EMAIL");
                entity.Property(x => x.CodEmpresa).HasColumnName("COD_EMPRESA");

                entity.HasMany(x => x.ClienteEnderecos).WithOne(x => x.Cliente);
            });

            modelBuilder.Entity<Endereco>(entity =>
            {
                entity.ToTable("TB_ENDERECO");
                entity.HasKey(e => e.ID);

                entity.Property(x => x.ID).HasColumnName("ID").ValueGeneratedOnAdd();
                entity.Property(x => x.Rua).HasColumnName("RUA");
                entity.Property(x => x.Bairro).HasColumnName("BAIRRO");
                entity.Property(x => x.Numero).HasColumnName("NUMERO");
                entity.Property(x => x.Complemento).HasColumnName("COMPLEMENTO");
                entity.Property(x => x.CEP).HasColumnName("CEP");
                entity.Property(x => x.TipoEndereco).HasColumnName("TIPO_ENDERECO");
                entity.Property(x => x.CidadeId).HasColumnName("TB_CIDADE_ID");

                entity.HasOne(e => e.Cidade);
                entity.HasMany(x => x.ClienteEnderecos).WithOne(x => x.Endereco);
            });

            modelBuilder.Entity<ClienteEndereco>(entity =>
            {
                entity.ToTable("TB_CLIENTE_ENDERECO");
                entity.HasKey(ce => ce.ID);

                entity.Property(x => x.ID).HasColumnName("ID").ValueGeneratedOnAdd();
                entity.Property(x => x.ClienteID).HasColumnName("TB_CLIENTE_ID");
                entity.Property(x => x.EnderecoID).HasColumnName("TB_ENDERECO_ID");

                entity.HasOne(e => e.Cliente);
                entity.HasOne(e => e.Endereco);
               
            });


            modelBuilder.Entity<Cidade>(entity =>
            {
                entity.ToTable("TB_CIDADE");
                entity.HasKey(c => c.ID);

                entity.Property(x => x.ID).HasColumnName("ID").ValueGeneratedOnAdd();
                entity.Property(x => x.Nome).HasColumnName("NOME");
                entity.Property(x => x.Estado).HasColumnName("ESTADO");

                entity.HasMany(x => x.Enderecos).WithOne(x => x.Cidade);
            });
        }
    }
}
