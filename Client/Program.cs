using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client.Components;
using Client.Identity;
using Client;
using Blazored.LocalStorage;
using MudBlazor.Services;
using Client.MyAccount;
using Client.Notifications;




var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<INotificationManager, NotificationManager>();

builder.Services.AddMudServices();
builder.Services.AddScoped<BearerHandler>();

// set up authorization
builder.Services.AddAuthorizationCore();

// register the custom state provider
builder.Services.AddScoped<AuthenticationStateProvider, BearerAuthenticationStateProvider>();

builder.Services.AddScoped<IMyAccount, MyAccountManager>();

// register the account management interface
builder.Services.AddScoped(
    sp => (IAccountManagement)sp.GetRequiredService<AuthenticationStateProvider>());




builder.Services.AddBlazoredLocalStorageAsSingleton();

// set base address for default host

builder.Services.AddHttpClient("Auth", client =>
{
    client.BaseAddress = new Uri("https://localhost:7189");
}).AddHttpMessageHandler<BearerHandler>();


await builder.Build().RunAsync();

