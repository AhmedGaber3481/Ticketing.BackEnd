using LinkDev.Ticketing.Logging.Infra;
using LinkDev.UserManagent.Application.Interfaces;
using LinkDev.UserManagent.Infrastructure.Data;
using LinkDev.UserManagent.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddScoped<IUserManagerRepository, UserManagerRepository>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.AllowedForNewUsers = true;

    //options.SignIn.RequireConfirmedPhoneNumber = false;
    //options.SignIn.RequireConfirmedEmail = false;

    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

// Authentication Cookie Name ".AspNetCore.Identity.Application"

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;   // IMPORTANT
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // REQUIRED
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
});

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

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseCors("AllowOrigin");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
