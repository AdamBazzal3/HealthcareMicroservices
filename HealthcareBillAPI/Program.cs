
using Consul;
using HealthcareBillAPI.Models;

using HealthcareBillAPI.Services.DbProxyService;
using Newtonsoft.Json.Serialization;
using Polly;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHostedService<ConsulServiceRegistration>();
builder.Services.AddControllers()
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ContractResolver = new DefaultContractResolver();
        });
builder.Services.AddControllers();
builder.Services.AddScoped<IDbProxy<Bill>, MongoDbProxy<Bill>>();
builder.Services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
{
    consulConfig.Address = new Uri("http://consul:8500");
}));
var app = builder.Build();
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.Run();
