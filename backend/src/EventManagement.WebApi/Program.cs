using EventManagement.BusinessLogicLayer;
using EventManagement.DataAccessLayer;
using EventManagement.Infrastructure;
using EventManagement.WebApi;
using EventManagement.WebApi.Configurations;
using EventManagement.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDataAccessServices(builder.Configuration);
builder.Services.RegisterBusinessLogicLayer();
builder.Services.AddInfrastructureServices();
builder.Services.AddAPIServices(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
await app.MigrateAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseErrorHandlingMiddleware();
app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();