using System.Reflection;
using System.Text.Json.Serialization;
using FunBooksAndVideos.Api.ExceptionHandling;
using FunBooksAndVideos.Application;
using FunBooksAndVideos.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FunBooksAndVideos API",
        Version = "v1",
        Description = "E-commerce API for books, online videos and club memberships. " +
                      "Creating a purchase order runs it through the purchase order processor " +
                      "(BR1: activate memberships, BR2: generate shipping slips)."
    });

    var xmlFile = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
    if (File.Exists(xmlFile))
    {
        options.IncludeXmlComments(xmlFile);
    }
});

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// IoC: each layer registers its own services.
builder.Services
    .AddApplication()
    .AddInfrastructure();

var app = builder.Build();

app.UseExceptionHandler();

// Swagger is enabled in every environment so the exercise is easy to review.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "FunBooksAndVideos API v1");
    options.DocumentTitle = "FunBooksAndVideos API";
});

app.UseHttpsRedirection();
app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.Run();

// Makes Program visible to WebApplicationFactory in the integration tests.
public partial class Program
{
}
