using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Blazored.LocalStorage;
using Client.Notifications;
using MudBlazor.Services;
using Client.Components;
using Client.MyAccount;
using Client.Identity;
using Client.Queue;
using Client;



var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<INotificationManager, NotificationManager>();
builder.Services.AddScoped<IQueueManager, QueueManager>();

builder.Services.AddMudServices();
builder.Services.AddScoped<BearerHandler>();

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, BearerAuthenticationStateProvider>();

builder.Services.AddScoped<IMyAccount, MyAccountManager>();

builder.Services.AddScoped(
    sp => (IAccountManagement)sp.GetRequiredService<AuthenticationStateProvider>());

builder.Services.AddBlazoredLocalStorageAsSingleton();

builder.Services.AddHttpClient("Auth", client =>
{
    client.BaseAddress = new Uri("https://localhost:7189");
}).AddHttpMessageHandler<BearerHandler>();


await builder.Build().RunAsync();

