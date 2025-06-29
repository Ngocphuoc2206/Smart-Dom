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
        public DbSet<RoomModel> Rooms { get; set; } = null!;
        public DbSet<CheckInHistoryModel> CheckInHistories { get; set; } = null!;
        public DbSet<InvoiceModel> Invoices { get; set; } = null!;
        public DbSet<AccountModel> Accounts { get; set; } = null!;
        public DbSet<ContractModel> Contracts { get; set; } = null!;
        public DbSet<MaintenanceRequestModel> MaintenanceRequests { get; set; } = null!;
        public DbSet<RoomBookingModel> RoomBookings { get; set; } = null!;
        public DbSet<DurationContractModel> DurationContracts { get; set; } = null!;
        public DbSet<NotificationModel> Notifications { get; set; }
        public DbSet<ReviewImageModel> ReviewsImages { get; set; }
        public DbSet<RoomReviewModel> RoomReviews { get; set;} = null!;
        public DbSet<ChatMessageModel> ChatMessages { get; set; }
        public DbSet<TransactionModel> Transactions { get; set; }

    }
}
