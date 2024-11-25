
using Consul;
using HealthcareClinicAPI.Models;

using HealthcareClinicAPI.Services.DbProxyService;
using Newtonsoft.Json.Serialization;
using Polly;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHostedService<ConsulServiceRegistration>();
// Add services to the container.
//builder.Services.AddHostedService<ConsulServiceRegistration>();
//builder.Services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
//{
//    consulConfig.Address = new Uri("http://consul:8500");
//}));
//builder.Services.AddSingleton(Polly.Policy
//       .Handle<Exception>()
//       .CircuitBreaker(
//           exceptionsAllowedBeforeBreaking: 3,
//           durationOfBreak: TimeSpan.FromMinutes(1)
//       ));
builder.Services.AddControllers()
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ContractResolver = new DefaultContractResolver();
        });
builder.Services.AddControllers();
builder.Services.AddScoped<IDbProxy<Doctor>, MongoDbProxy<Doctor>>();
builder.Services.AddScoped<IDbProxy<MedicalRecord>, MongoDbProxy<MedicalRecord>>();
builder.Services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
{
    consulConfig.Address = new Uri("http://consul:8500");
}));
//builder.Services.Configure<HealthcareDbSettings>(
//    builder.Configuration.GetSection("HealthcareDatabase"));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.Run();
