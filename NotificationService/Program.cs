
using Contracts.Common;
using MassTransit;
using NotyficationService;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddSingleton<NotificationHub, NotificationHub>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:5026", "https://localhost:7284")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin();
        });
});


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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapHub<NotificationHub>("notyfications");


app.UseCors("AllowMyOrigin");



app.Run();

