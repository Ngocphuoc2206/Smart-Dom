using Microsoft.EntityFrameworkCore;

namespace Smart_Dom.Models
{
    public class AppDBContext: DbContext
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //foreach (var entitytype in modelBuilder.Model.GetEntityTypes())
            //{
            //    var tablename = entitytype.GetTableName();
            //    if (tablename.StartsWith("AspNet"))
            //    {
            //        entitytype.SetTableName(tablename[6..]);
            //    }
            //}
        }

        public DbSet<UserModel> Users { get; set; } = null!;
    }
}
