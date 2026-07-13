using ClientPortal.Application;
using ClientPortal.Infrastructure;
using ClientPortal.WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

//Register services
builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("Postgres")!);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<ExceptionHandlerMiddleware>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers();

app.Run();

