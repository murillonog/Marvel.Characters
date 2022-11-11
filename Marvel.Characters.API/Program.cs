using Marvel.Characters.Infra.IoC;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddInfrastructureAPI(configuration);

var app = builder.Build();
app.Run();
