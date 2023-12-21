
using NotyficationService;


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
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapHub<NotificationHub>("notyfications");

app.MapPost("/notifyClient", (string userGuid, NotificationHub hub) =>
{
    var userId = new UserIdDto(userGuid);
    hub.NotifyUser(userId);
    return;
})
.WithName("NotifyClient")
.WithOpenApi();

app.UseCors("AllowMyOrigin");



app.Run();

