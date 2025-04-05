using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DeventSoft.RoadmapSkills.Users.Infrastructure;

namespace DeventSoft.RoadmapSkills.Users.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddFastEndpoints();
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();
        
        // Add infrastructure services
        builder.Services.AddUsersInfrastructure(builder.Configuration);
        
        // Configure Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c => {
            c.SwaggerDoc("v1", new() { Title = "DeventSoft RoadmapSkills Users API", Version = "v1" });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DeventSoft RoadmapSkills Users API v1");
            });
        }

        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseFastEndpoints(c => {
            c.Endpoints.RoutePrefix = "api";
        });

        app.Run();
    }
}
