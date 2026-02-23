using LinkDev.Ticketing.Application.Interfaces;
using LinkDev.Ticketing.Application.IServices;
using LinkDev.Ticketing.Application.Mapping;
using LinkDev.Ticketing.Application.Services;
using LinkDev.Ticketing.Core.Helpers;
using LinkDev.Ticketing.Infrastructure.Data;
using LinkDev.Ticketing.Infrastructure.Helpers;
using LinkDev.Ticketing.Infrastructure.Repositories;
using LinkDev.Ticketing.Infrastructure.Uow;
using LinkDev.Ticketing.Logging.Infra;
using Microsoft.EntityFrameworkCore;
using Serilog;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var loggingSection = builder.Configuration.GetSection("CustomLogging");
        if (loggingSection != null)
        {
            Serilog.ILogger logger = new LoggerBuilder().Initialize(loggingSection).ConfigureLoggingSink(loggingSection).CreateLogger();
            builder.Logging.AddSerilog(logger, true);
            builder.Services.AddSingleton<LinkDev.Ticketing.Logging.Application.Interfaces.ILogger, Logger>();
        }

        builder.Services.AddControllers();

        builder.Services.AddDbContext<TicketingContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("TicketingConnection")));

        builder.Services.AddMemoryCache();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped(typeof(LinkDev.Ticketing.Application.Interfaces.IRepository<>), typeof(LinkDev.Ticketing.Infrastructure.Repositories.Repository<>));
        builder.Services.AddScoped<ITicketService, TicketService>();
        builder.Services.AddSingleton<DirectoryManager>();
        builder.Services.AddSingleton<FileManager>();
        builder.Services.AddScoped<ITicketRepository, TicketRepository>();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<CultureHelper>();
        builder.Services.AddScoped<ILookupRepository, LookupRepository>();
        builder.Services.AddScoped<ILookupService, LookupService>();
        builder.Services.AddSingleton<DBHelper>();

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

        app.UseAuthorization();

        app.MapControllers();

        //MappingConfig.RegisterMappings();

        app.Run();
    }
}