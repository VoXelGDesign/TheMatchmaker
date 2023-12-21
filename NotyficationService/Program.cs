using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NotyficationService;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

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
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("/notifyClient", async (string userGuid, IHubContext<NotificationHub, INotificationClient> _context, ClaimsPrincipal _user) =>
{
    
    await _context.Clients.All.RecieveNotification();
    _user.FindFirstValue(ClaimTypes.NameIdentifier);
    return;
})
.WithName("NotifyClient")
.WithOpenApi();

app.UseCors("AllowMyOrigin");
app.MapHub<NotificationHub>("notyfications");


app.Run();

