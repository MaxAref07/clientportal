using ClientPortal.Application;
using ClientPortal.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

//Register services
builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.Run();

