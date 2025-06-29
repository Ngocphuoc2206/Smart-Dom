using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Smart_Dom.Hubs;
using Smart_Dom.Hubs.ChatRealTime;
using Smart_Dom.Interfaces;
using Smart_Dom.Repositories;
using Smart_Dom.Services;

namespace Smart_Dom
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Cấu hình kết nối cơ sở dữ liệu với Entity Framework Core
            builder.Services.AddDbContext<Models.AppDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("AppDBContext")));

            //Đăng ký Signal
            builder.Services.AddSignalR(options =>
            {
                options.KeepAliveInterval = TimeSpan.FromSeconds(15); // ping giữ kết nối
            });
            builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();


            // Cấu hình Repository
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddScoped<IRoomBookingRepository, RoomBookingRepository>();
            builder.Services.AddScoped<IContractRepository, ContractRepository>();
            builder.Services.AddScoped<ICheckInHistoryRepository, CheckInHistoryRepository>();
            builder.Services.AddScoped<IDurationContractRepository, DurationContractRepository>();
            builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<IMaintenanceRequestRepository, MaintenanceRequestRepository>();
            builder.Services.AddScoped<IRoomReviewRepository, RoomReviewRepository>();
            builder.Services.AddScoped<IChatMessageRepository, ChatMessageRepository>();


            // Cấu hình Services
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRoomService, RoomService>();
            builder.Services.AddScoped<IRoomBookingService, RoomBookingService>();
            builder.Services.AddScoped<IContractService, ContractService>();
            builder.Services.AddScoped<ICheckInHistoryService, CheckInHistoryService>();
            builder.Services.AddScoped<IDurationContracService, DurationContractService>();
            builder.Services.AddScoped<IInvoiceService, InvoiceService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IMaintenanceRequestService, MaintenaceRequestService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IRoomReviewService, RoomReviewService>();
            builder.Services.AddScoped<IChatMessageService, ChatMessageService>();

            // Cấu hình cors nhúng nextjs
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowNextJs",
                    builder => builder.WithOrigins("http://localhost:3000")
                                      .AllowAnyMethod()
                                      .AllowAnyHeader()
                                      .AllowCredentials());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseRouting();
            app.UseCors("AllowNextJs");
            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            // Top-level route registrations
            app.MapHub<NotificationHub>("/hub/notifications");
            app.MapHub<ChatHub>("/chatHub");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
