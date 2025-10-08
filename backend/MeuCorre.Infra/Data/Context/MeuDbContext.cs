using MeuCorre.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeuCorre.Infra.Data.Context
{
    public class MeuDbContext : DbContext
    {
        public MeuDbContext(
            DbContextOptions<MeuDbContext> opcoes) : base(opcoes)
        {
            //Desabilita o rastreamento de alterações para melhorar a
            //performance em consultas somente leitura.
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        //Define a ligação entre a classe c# com a tabela do DB.
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public  DbSet<Conta> Contas { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplica as configurações de mapeamento das entidades
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(MeuDbContext).Assembly);
        }
    }
}
