using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using KubbAdminAPI;
using KubbAdminAPI.Services;
using KubbAdminAPI.Singletons;
using KubbAdminAPI.Utils;
using KubbAdminAPI.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
if (builder.Configuration["SSL:Enable"] == "True")
{
    builder.WebHost.ConfigureKestrel((context, serverOptions) =>
    {
        serverOptions.ListenAnyIP(443, listenOptions =>
        {
            var pemPath = builder.Configuration["SSL:CertPath"]!;
            var keyPath = builder.Configuration["SSL:KeyPath"]!;
        
            var certificate = X509Certificate2.CreateFromPemFile(pemPath, keyPath);

            listenOptions.UseHttps(certificate);

        });
    });
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database service
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("POSTGRES"));
});
// Cache service
builder.Services.AddMemoryCache();

// Adding worker for scoreboard generation
builder.Services.AddHostedService<ScoreboardWorker>();
// builder.Services.AddHostedService<FreezerWorker>();
builder.Services.AddScoped<EmailService>();

// Cloudflare Turnstile
builder.Services.AddScoped<TurnstileService>();

builder.Services.AddHostedService<EmailSenderWorker>();

// cleaner for DB
builder.Services.AddHostedService<CleanerWorker>();

// Email Queue Singleton
builder.Services.AddSingleton<EmailTask>();

// Adding CORS for Debug
builder.Services.AddCors(options =>
{
    options.AddPolicy("cors.def", x =>
    {
        x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("cors.def");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
// app.UseAuthorization();

// static files
StaticFileOptions staticFileOptions = new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Static")),
    RequestPath = "/Assets",
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=2592000");
        ctx.Context.Response.Headers.Append("Expires",
            DateTime.UtcNow.AddDays(30).ToString("R", CultureInfo.InvariantCulture));
    },
};

app.UseStaticFiles(staticFileOptions);
app.MapFallbackToFile("index.html", new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Static"))
});

app.MapControllers();


app.Run();
