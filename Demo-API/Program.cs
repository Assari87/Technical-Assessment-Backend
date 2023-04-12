using Demo_API.Controllers;
using Demo_Models.Models;
using Demo_Services.Services.AssetServices;
using Demo_Services.Services.DateTimeServices;
using Demo_Services.Services.RemoteAssetServices;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();
builder.Services.AddSingleton<SystemConfigs>(provider => builder.Configuration.GetSection("SystemConfigs").Get<SystemConfigs>());
builder.Services.AddScoped<IDateTimeService, DateTimeService>();
builder.Services.AddSingleton<IRemoteAssetService, RemoteAssetService>();
builder.Services.AddScoped<IAssetService, AssetService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();