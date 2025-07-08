using BLL.Interfaces;
using BLL.Services;
using DAL;
using DAL.Interfaces;
using DAL.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace BookBorrowingSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            //-------------------------------------------- Tạo và cấu hình DbContext với SQL Server --------------------------------------------
            builder.Services.AddDbContext<LibraryDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            //-------------------------------------------- Authentication and Authorization --------------------------------------------
            builder .Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login"; // Đường dẫn đến trang đăng nhập
                    options.LogoutPath = "/Authen/Login/Logout"; // Đường dẫn đến trang đăng xuất
                    options.AccessDeniedPath = "/Authen/Login/Index"; // Đường dẫn đến trang từ chối truy cập
                });
            //-------------------------------------------- Account ---------------------------------------------------------------------
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAccountRepo, AccountRepo>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication(); // Thêm middleware xác thực trước khi sử dụng Authorization

            app.UseAuthorization();

            app.MapRazorPages();
            //-------------------------------------------- Set trang đầu là Login --------------------------------------------
            app.MapGet("/", context =>
            {
                context.Response.Redirect("/Login");
                return Task.CompletedTask;
            });

            app.Run();
        }
    }
}
