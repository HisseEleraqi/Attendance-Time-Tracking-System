using AttendenceSystem.IRepo;
using AttendenceSystem.Repo;

using AttendenceSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Hangfire;

namespace AttendenceSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.



          
            builder.Services.AddControllersWithViews();

            // Add the Hangfire
            string connectionString = "Server =.; database = AttendenceDB; integrated security = true; trustservercertificate = true";
            string connString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(connString));

            // Add the Hangfire server
            builder.Services.AddHangfireServer();
            // End the Hangfire

            builder.Services.AddTransient<InstructorIRepo, InstructorRepo>();

            builder.Services.AddScoped<IEmpRepo, EmpRepo>();
            builder.Services.AddScoped<TrackIRepo, TrackRepo>();
            builder.Services.AddScoped<IAttendance , Attendance>();         



            builder.Services.AddScoped<IAccountRepo, AccountRepo>();
            builder.Services.AddScoped<IStudentRepo, StudentRepo>();

            builder.Services.AddScoped<IReportService, ReportService>();
            
            builder.Services.AddSession();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHangfireDashboard();


            var student = new StudentRepo();

            RecurringJob.AddOrUpdate(() => student.GetAllUsers(), Cron.Daily(0));


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Student}/{action=StudentScdule}/{id?}");

            app.Run();
        }
    }
}
