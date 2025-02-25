using KubbAdminAPI;
using KubbAdminAPI.Utils;
using KubbAdminAPI.Workers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database service
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("POSTGRES"));
});

// Mail service
builder.Services.AddTransient<EmailService>();
// Cache service
builder.Services.AddMemoryCache();

// Adding worker for scoreboard generation
// builder.Services.AddHostedService<ScoreboardWorker>();
// builder.Services.AddHostedService<FreezerWorker>();
builder.Services.AddHostedService<EmailSenderWorker>();

// Add Redis caching
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "MyApp_";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();