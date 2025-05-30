using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DeventSoft.RoadmapSkills.Users.Infrastructure;
using DeventSoft.RoadmapSkills.Users.Api.Extensions;

namespace DeventSoft.RoadmapSkills.Users.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddUsersInfrastructure(builder.Configuration);
        builder.Services.AddFastEndpoints();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Add authentication
        builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = builder.Configuration["Authentication:Authority"];
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new()
                {
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });

        // Add authorization
        builder.Services.AddAuthorization();

        builder.Services.AddUsersModule();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseFastEndpoints(c => {
            c.Endpoints.RoutePrefix = "api";
        });

        app.UseUsersModule();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/error");
            app.UseHsts();
        }

        app.Run();
    }
}
