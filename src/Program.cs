using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MinimalApi_SmartLog.Data;
using NetDevPack.Identity.Jwt;
using MinimalApi_SmartLog.Models;
using Microsoft.AspNetCore.Authorization;
using MiniValidation;

var builder = WebApplication.CreateBuilder(args);

#region Configure services

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Minimal API Sample",
        Description = "",
        License = new OpenApiLicense { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insert the JWT token: Bearer {your token}",
        Name = "Authorization",
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<MinimalContextDb>(options =>
    options.UseSqlServer(builder.Configuration["DefaultConnection"]));

builder.Services.AddIdentityEntityFrameworkContextConfiguration(options =>
    options.UseSqlServer(builder.Configuration["DefaultConnection"],
    b => b.MigrationsAssembly("MinimalApi-CustomerRecords")));

builder.Services.AddIdentityConfiguration();
builder.Services.AddJwtConfiguration(builder.Configuration, "Jwt");

var app = builder.Build();

#endregion

#region Configure pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthConfiguration();
app.UseHttpsRedirection();

MapActions(app);

app.Run();

#endregion

#region Endpoints

void MapActions(WebApplication app)
{
    app.MapGet("/logs", [Authorize] async (
        MinimalContextDb context) =>

        await context.Logs.ToListAsync())
        .Produces<Log>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status204NoContent)
        .WithName("GetLogs")
        .WithTags("Logs");

    app.MapGet("/logs/{id}", [Authorize] async (
        Guid id,
        MinimalContextDb context) =>

        await context.Logs.FindAsync(id)
            is Log log
                ? Results.Ok(log)
                : Results.NotFound())
        .Produces<Log>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetLogById")
        .WithTags("Logs");

    app.MapPost("/log", [Authorize] async (
        Log log, MinimalContextDb context) =>
        {
            if (!MiniValidator.TryValidate(log, out var errors))
                return Results.ValidationProblem(errors);

            context.Logs.Add(log);
            var result = await context.SaveChangesAsync();

            return result > 0
                ? Results.CreatedAtRoute("GetCustomerById", new { id = log.Id }, log)
                : Results.BadRequest("An error ocurred while saving the record");
        })
        .ProducesValidationProblem()
        .Produces<Log>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithName("PostLog")
        .WithTags("Logs");
}

#endregion