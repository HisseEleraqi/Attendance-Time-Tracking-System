using AttendenceSystem.IRepo;
using AttendenceSystem.Repo;

using AttendenceSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using AttendenceSystem.CustomFilter;

namespace AttendenceSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

         

          
            builder.Services.AddTransient<InstructorIRepo, InstructorRepo>();
            builder.Services.AddTransient<IEmpRepo, EmpRepo>();
            builder.Services.AddTransient<TrackIRepo, TrackRepo>();


            builder.Services.AddTransient<IAccountRepo, AccountRepo>();
            builder.Services.AddTransient<IStudentRepo, StudentRepo>();
            builder.Services.AddTransient<IStudentService, StudentService>();
            builder.Services.AddTransient<IUserRepo, UserRepo>();
            builder.Services.AddSession();
            builder.Services.AddControllersWithViews(option => 
            { option.Filters.Add<AuthFilter>();
            });
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie
                (option =>
                {
                    option.AccessDeniedPath = "/Account/AccessError";
                    option.LoginPath = "/Account/Login";
                }

                );
            
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",

                pattern: "{controller=Student}/{action=DisplayAllStudent}");


            app.Run();
        }
    }
}
