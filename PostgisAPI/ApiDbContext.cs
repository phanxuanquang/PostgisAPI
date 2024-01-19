using Microsoft.EntityFrameworkCore;
using PostgisAPI.Models;

namespace PostgisAPI
{
    public class ApiDbContext : DbContext
    {
        public DbSet<ModelItem> ModelItems { get; set; }
        public DbSet<Model> Models { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Model>().ToTable("model").HasKey(m => m.ModelID);

            modelBuilder.Entity<ModelItem>().ToTable("modelitem").HasKey(m => m.ModelItemID);

            modelBuilder.Entity<Model>().Ignore(m => m.ID);
            modelBuilder.Entity<Model>().Ignore(m => m.LastModifiedTime);

            modelBuilder.Entity<ModelItem>().Ignore(m => m.ID);
            modelBuilder.Entity<ModelItem>().Ignore(m => m.LastModifiedTime);

            base.OnModelCreating(modelBuilder);
        }
    }
}
