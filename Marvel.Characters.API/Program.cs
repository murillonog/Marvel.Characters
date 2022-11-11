using Marvel.Characters.Infra.IoC;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddInfrastructureAPI(configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddInfrastructureSwagger();

var app = builder.Build();

app.MapGet("/", () => "Olá Mundo!");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
