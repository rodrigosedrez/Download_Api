using Download.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Download.Infrastructure.Context
{
    public class DownloadsDBContext : IdentityDbContext<IdentityUser>
    {
        protected DownloadsDBContext()
        {
        }

        public DownloadsDBContext(DbContextOptions<DownloadsDBContext> options) : base(options)
        {
        }

        public class DownloadsDBContextFactory : IDesignTimeDbContextFactory<DownloadsDBContext>
        {
            public DownloadsDBContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<DownloadsDBContext>();
                optionsBuilder.UseSqlServer("Server=sqlserver,1433;Database=DownloadsAPIDocker;User Id=SA;Password=You12Pass34.;Encrypt=True;TrustServerCertificate=True;");

                return new DownloadsDBContext(optionsBuilder.Options);
            }
        }

        public DbSet<GameModel> GameModels { get; set; }
        public DbSet<EmulatorsModel> Emulators { get; set; }
        public DbSet<ExternalLinksModel> ExternalLinksModels { get; set; }
        public DbSet<MagazineModel> MagazineModels { get; set; }
        public DbSet<PlataformModels> PlataformModels { get; set; }
        public DbSet<GameCategoryModel> GameCategoryModels { get; set; }
        public DbSet<PostModel> PostModels { get; set; }
        public DbSet<Publicity> Publicitys { get; set; }
        public DbSet<MisturebaModel> MisturebaModels { get; set; }

        public DbSet<GameTagModel> GameTagModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DownloadsDBContext).Assembly);
            var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            modelBuilder.Entity<MisturebaModel>().HasData(new MisturebaModel
            {
                Id = 1,
                Name = "appsettings.json",
                LocalOndeFica = path,
                Segredo = "Arquivo de configuração da aplicação"
            });

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}