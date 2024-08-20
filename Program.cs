using Microsoft.Extensions.Configuration;
using Polly.Retry;
using WebApplication1.Helpers;
using WebApplication1.Interfaces;
using WebApplication1.Service;

var builder = WebApplication.CreateBuilder(args);



builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

builder.Services.AddHttpClient();
builder.Services.AddScoped<IAirportService,AirportService>();
builder.Services.AddSingleton<IRetryPolicyService, RetryPolicyService>();



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
