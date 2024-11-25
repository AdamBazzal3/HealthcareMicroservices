using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using HealthcareUI.AuthenticationState;
using HealthcareUI.Events;
using HealthcareUI.Services.RestService;
using Consul;
using HealthcareIdentityAPI.Services;
using HealthcareUI.Services;
using HealthcareUI.Models;
using HealthcareIdentityAPI.Models;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
{
    consulConfig.Address = new Uri("http://consul:8500");

}));
builder.Services.AddSingleton<ServiceDiscovery>();
// Add services to the container.
builder.Services.AddAuthenticationCore();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient<IDataService<Patient>, RestService<Patient>>().ConfigureHttpClient(async (provider, client) =>
{
    var serviceDiscovery = provider.GetRequiredService<ServiceDiscovery>();
    string origin = await serviceDiscovery.GetServiceUriAsync(AppConstants.PATIENT_SERVICE_NAME.ToString()); 
    client.BaseAddress = new Uri($"{origin}"); ;
});
builder.Services.AddHttpClient<IDataService<Appointment>, RestService<Appointment>>().ConfigureHttpClient(async (provider, client) =>
{
    var serviceDiscovery = provider.GetRequiredService<ServiceDiscovery>();
    string origin = await serviceDiscovery.GetServiceUriAsync(AppConstants.APPOINTMENT_SERVICE_NAME.ToString()) ?? throw new Exception("Service is not registered");
    client.BaseAddress = new Uri($"{origin}"); ;
});
builder.Services.AddHttpClient<IDataService<Doctor>, ClinicRestService<Doctor>>().ConfigureHttpClient(async (provider, client) =>
{
    var serviceDiscovery = provider.GetRequiredService<ServiceDiscovery>();
    string origin = await serviceDiscovery.GetServiceUriAsync(AppConstants.CLINICAL_SERVICE_NAME.ToString()) ?? throw new Exception("Service is not registered");
    client.BaseAddress = new Uri($"{origin}"); ;
});
builder.Services.AddHttpClient<IDataService<Prescription>, ClinicRestService<Prescription>>().ConfigureHttpClient(async (provider, client) =>
{
    var serviceDiscovery = provider.GetRequiredService<ServiceDiscovery>();
    string origin = await serviceDiscovery.GetServiceUriAsync(AppConstants.CLINICAL_SERVICE_NAME.ToString()) ?? throw new Exception("Service is not registered");
    if (string.IsNullOrEmpty(origin)) return;
    client.BaseAddress = new Uri($"{origin}"); ;
});
builder.Services.AddHttpClient<IDataService<MedicalRecord>, ClinicRestService<MedicalRecord>>().ConfigureHttpClient(async (provider, client) =>
{
    var serviceDiscovery = provider.GetRequiredService<ServiceDiscovery>();
    string origin = await serviceDiscovery.GetServiceUriAsync(AppConstants.CLINICAL_SERVICE_NAME.ToString()) ?? throw new Exception("Service is not registered");
    client.BaseAddress = new Uri($"{origin}"); ;
});
builder.Services.AddHttpClient<IDataService<Bill>, RestService<Bill>>().ConfigureHttpClient(async (provider, client) =>
{
    var serviceDiscovery = provider.GetRequiredService<ServiceDiscovery>();
    string origin = await serviceDiscovery.GetServiceUriAsync(AppConstants.BILL_SERVICE_NAME.ToString()) ?? throw new Exception("Service is not registered");
    client.BaseAddress = new Uri($"{origin}");
});
builder.Services.AddScoped<StateContainer>();
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddSingleton<InvoiceEventPublisher>();
builder.Services.AddScoped<AuthenticationStateProvider, HealthcareAuthStateProvider>();
builder.Services.AddHttpClient<IAuthService, AuthRestService>();
builder.Services.AddSingleton<ServiceDiscovery>();
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
//builder.WebHost.UseWebRoot("wwwroot");
//builder.WebHost.UseStaticWebAssets();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
