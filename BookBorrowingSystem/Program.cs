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
            // Add signalR
            builder.Services.AddSignalR();

            //-------------------------------------------- Tạo và cấu hình DbContext với SQL Server --------------------------------------------
            builder.Services.AddDbContext<LibraryDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            //-------------------------------------------- Authentication and Authorization --------------------------------------------
            builder .Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Authen/Login/Index"; // Đường dẫn đến trang đăng nhập
                    options.LogoutPath = "/Authen/Logout/Logout"; // Đường dẫn đến trang đăng xuất
                    options.AccessDeniedPath = "/Authen/AccessDenied/Index"; // Đường dẫn đến trang từ chối truy cập
                });
            //-------------------------------------------- Account ---------------------------------------------------------------------
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAccountRepo, AccountRepo>();

            //-------------------------------------------- Book ------------------------------------------------------------------------
            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddScoped<IBookRepo, BookRepo>();
            //-------------------------------------------- Request --------------------------------------------------------------------
            builder.Services.AddScoped<IRequestService, RequestService>();
            builder.Services.AddScoped<IRequestRepo, RequestRepo>();

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
            // Thêm SignalR Hub mapping
            app.MapHub<Pages.Hubs.LibraryHub>("/libraryHub");
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
