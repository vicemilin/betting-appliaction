using Microsoft.Extensions.Configuration;
using VM.BettingApplication.API.Services;
using VM.BettingApplication.Core.Services.Implementation;
using VM.BettingApplication.Core.Services.Interface;
using VM.BettingApplication.Core.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IOfferService, OfferService>();
builder.Services.AddSingleton<ISportService, SportService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddHostedService<OfferHostedService>();
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
