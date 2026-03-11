using LinkDev.Ticketing.Application.Interfaces;
using LinkDev.Ticketing.Application.IServices;
using LinkDev.Ticketing.Application.Services;
using LinkDev.Ticketing.Core.Helpers;
using LinkDev.Ticketing.Infrastructure.Data;
using LinkDev.Ticketing.Infrastructure.Helpers;
using LinkDev.Ticketing.Infrastructure.Repositories;
using LinkDev.Ticketing.Infrastructure.Uow;
using LinkDev.Ticketing.Logging.Infra;
using LinkDev.UserManagent.Application.Interfaces;
using LinkDev.UserManagent.Application.Services;
using LinkDev.UserManagent.Infrastructure.Data;
using LinkDev.UserManagent.Infrastructure.Repositories;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        string connectionString = builder.Configuration.GetConnectionString("TicketingConnection")!;

        builder.Services.AddDbContext<TicketingContext>(options => options.UseSqlServer(connectionString));

        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddAuthorization();

        builder.Services.AddMemoryCache();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddScoped<ITicketService, TicketService>();
        builder.Services.AddSingleton<DirectoryManager>();
        builder.Services.AddSingleton<FileManager>();
        builder.Services.AddScoped<ITicketRepository, TicketRepository>();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<CultureHelper>();
        builder.Services.AddScoped<ILookupRepository, LookupRepository>();
        builder.Services.AddScoped<ILookupService, LookupService>();
        builder.Services.AddSingleton<DBHelper>();

        string? PresistIdentityKeysPath = builder.Configuration["Security:PresistIdentityKeysPath"];
        if (PresistIdentityKeysPath != null)
        {
            builder.Services.AddDataProtection()
                    .PersistKeysToFileSystem(new DirectoryInfo(PresistIdentityKeysPath))
                    .SetApplicationName("SharedIdentityCookie");
        }

        builder.Services.AddScoped<IUserManager, UserManager>();
        builder.Services.AddScoped<ILoggedUserService, LoggedUserService>();

        var loggingSection = builder.Configuration.GetSection("CustomLogging");
        if (loggingSection != null)
        {
            Serilog.ILogger logger = new LoggerBuilder().Initialize(loggingSection).ConfigureLoggingSink(loggingSection).CreateLogger();
            builder.Logging.AddSerilog(logger, true);
            builder.Services.AddSingleton<LinkDev.Ticketing.Logging.Application.Interfaces.ILogger, Logger>();
        }

        var CrossOrigin = builder.Configuration["CorsOrigin"];

        if (!string.IsNullOrEmpty(CrossOrigin))
        {
            builder.Services.AddCors(options =>
            {

                options.AddPolicy("AllowOrigin", builder =>
                {
                    builder
                        .WithOrigins(CrossOrigin.Split(',', StringSplitOptions.RemoveEmptyEntries))
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }

        var app = builder.Build();

        app.UseCors("AllowOrigin");

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}