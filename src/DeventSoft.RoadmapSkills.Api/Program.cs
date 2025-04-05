using System.Reflection;
using DeventSoft.RoadmapSkills.Composition;
using DeventSoft.RoadmapSkills.Users.Application.Services;
using DeventSoft.RoadmapSkills.Users.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "RoadmapSkills API",
        Description = "An ASP.NET Core Web API for managing RoadmapSkills",
        Contact = new OpenApiContact
        {
            Name = "DeventSoft",
            Email = "contact@deventsoft.com"
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    options.EnableAnnotations();
    
    // Include XML comments
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// Register modules
builder.Services.AddRoadmapSkills(builder.Configuration);

var app = builder.Build();

// Apply database migrations
await app.Services.MigrateRoadmapSkillsDatabasesAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty; // Serve the Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();

// User endpoints
var users = app.MapGroup("/api/users");

users.MapGet("/", async (IUserService userService) =>
    {
        var result = await userService.GetAllUsersAsync();
        return Results.Ok(result);
    })
    .WithName("GetUsers")
    .WithOpenApi()
    .WithTags("Users")
    .Produces<IEnumerable<User>>(200);

users.MapGet("/{id}", async (Guid id, IUserService userService) =>
    {
        var user = await userService.GetUserByIdAsync(id);
        return user is null ? Results.NotFound() : Results.Ok(user);
    })
    .WithName("GetUserById")
    .WithOpenApi()
    .WithTags("Users")
    .Produces<User>(200)
    .Produces(404);

users.MapPost("/", async (User user, IUserService userService) =>
    {
        await userService.CreateUserAsync(user);
        return Results.Created($"/api/users/{user.Id}", user);
    })
    .WithName("CreateUser")
    .WithOpenApi()
    .WithTags("Users")
    .Produces<User>(201)
    .ProducesValidationProblem();

users.MapPut("/{id}", async (Guid id, User user, IUserService userService) =>
    {
        if (id != user.Id)
            return Results.BadRequest();
            
        var updated = await userService.UpdateUserAsync(user);
        return updated ? Results.NoContent() : Results.NotFound();
    })
    .WithName("UpdateUser")
    .WithOpenApi()
    .WithTags("Users")
    .Produces(204)
    .Produces(404)
    .ProducesValidationProblem();

users.MapDelete("/{id}", async (Guid id, IUserService userService) =>
    {
        var deleted = await userService.DeleteUserAsync(id);
        return deleted ? Results.NoContent() : Results.NotFound();
    })
    .WithName("DeleteUser")
    .WithOpenApi()
    .WithTags("Users")
    .Produces(204)
    .Produces(404);

app.Run();
