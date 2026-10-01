using API.Cors;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddFrontendCors(builder.Configuration);

var app = builder.Build();



// app.MapGet("/", () => "Hello World!");
app.UseCors(CorsExtensions.PolicyName);
app.MapControllers();
app.Run();
