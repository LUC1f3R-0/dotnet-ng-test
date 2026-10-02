using API;
using API.Cors;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAPI();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddFrontendCors(builder.Configuration);

var app = builder.Build();

app.UseCors(CorsExtensions.PolicyName);
app.MapControllers();
app.Run();
