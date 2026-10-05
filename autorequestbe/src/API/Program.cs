using API;
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAPI(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

app.UseAPI();
app.MapControllers();
app.Run();
