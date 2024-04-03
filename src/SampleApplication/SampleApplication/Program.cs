using SampleApplication.Extensions;
using SampleApplication.Workers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Definir cultura invariante
builder.Services.AddBaseServices(builder.Configuration);

// Adicione o serviço WeatherBackgroundService como um serviço hospedado
builder.Services.AddHostedService<WeatherBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddBaseApplication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
