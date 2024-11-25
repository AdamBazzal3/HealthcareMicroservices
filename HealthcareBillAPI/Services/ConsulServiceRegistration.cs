using Consul;
using HealthcareBillAPI.Models;
using HealthcareBillAPI.Utils;

public class ConsulServiceRegistration : IHostedService
{
    private readonly IConsulClient _consulClient;
    private readonly IHostApplicationLifetime _lifetime;
    private string _registrationId;

    public ConsulServiceRegistration(IConsulClient consulClient, IHostApplicationLifetime lifetime)
    {
        _consulClient = consulClient;
        _lifetime = lifetime;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        bool success = int.TryParse(Environment.GetEnvironmentVariable("BILL_API_PORT"), out int port);
        if (!success) return;
        // Service registration details
        var registration = new AgentServiceRegistration()
        {
            ID = $"HealthcareBillAPI-{Guid.NewGuid()}",
            Name = AppConstants.BILL_SERVICE_NAME.ToString(),
            Address = "HealthcareBillAPI", // Replace with your service's actual address
            Port = port // Replace with your service's actual port
        };

        _registrationId = registration.ID;
        await _consulClient.Agent.ServiceRegister(registration, cancellationToken);

        _lifetime.ApplicationStopping.Register(async () => await StopAsync(cancellationToken));
    }
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(_registrationId))
        {
            await _consulClient.Agent.ServiceDeregister(_registrationId, cancellationToken);
        }
    }
}