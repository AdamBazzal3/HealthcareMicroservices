using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using HealthcareIdentityAPI.Data;
using HealthcareIdentityAPI.Utils;
using System.Text;
using Microsoft.Extensions.Configuration;
using Consul;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHostedService<ConsulServiceRegistration>();
builder.Services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
{
    consulConfig.Address = new Uri("http://consul:8500");
}));
builder.Services.AddControllers();
builder.Logging
       .AddConsole()
       .AddDebug();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});
var connectionString = Environment.GetEnvironmentVariable("HEALTHCARE_IDENTITY_CONNECTIONSTRING");
builder.Services.AddDbContext<DataContext>(
options => options.UseSqlServer(connectionString,
sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null)
    )
);

builder.Services.AddSingleton<Crypto>();

//db migration



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<DataContext>();
    try
    {
        context.Database.Migrate();
    }
    catch (Exception)
    {
        await Task.Delay(2000);
        context.Database.Migrate();
    }
    if (!context.Users.Any(user => user.UserName.ToLower().Equals("admin")))
    {
        context.Users.Add(new() { Firstname = "Adam", Lastname = "Bazzal", UserName = "admin", Password = "admin123", Role = "Admin" });
        context.Users.Add(new() { Firstname = "Cashier", Lastname = "Bazzal", UserName = "96675", Password = "96675", Role = "Cashier" });

        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.


app.UseAuthentication();

app.MapControllers();

app.Run();
