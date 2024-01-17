using Contracts.ApiContracts.Queue.Requests;
using Contracts.QueueContracts.RocketLeague;
using MassTransit;
using QueueService;
using QueueService.Consumers;
using QueueService.Publishers.CreateRocketLeagueLobby;
using QueueService.Publishers.JoinedQueue;
using QueueService.Publishers.RemovedFromQueue;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ICreateRocketLeagueLobbyPublisher,
    CreateRocketLeagueLobbyPublisher>();


builder.Services.AddSingleton<IJoinedQueuePublisher,
    JoinedQueuePublisher>();

builder.Services.AddSingleton<IRemovedFromQueuePublisher,
    RemovedFromQueuePublisher>();

builder.Services.AddHostedService<RocketLeagueQueue>();

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    x.AddConsumers(Assembly.GetExecutingAssembly());

    x.UsingAzureServiceBus((context, cfg) =>
    {
        cfg.Host(builder.Configuration["AzureServiceBusConnectionString"]);
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();

