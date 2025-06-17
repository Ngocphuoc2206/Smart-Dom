using Microsoft.EntityFrameworkCore;

namespace Smart_Dom
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //Cấu hình kết nối cơ sở dữ liệu với Entity Framework Core
            builder.Services.AddDbContext<Models.AppDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("AppDBContext")));

            //Cấu hình cors nhúng nextjs
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowNextJs",
                    builder => builder.WithOrigins("http://localhost:3000")
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
