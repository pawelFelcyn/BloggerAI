using BloggerAI.API;
using BloggerAI.API.Middleware;
using BloggerAI.Core;
using BloggerAI.Core.Authentication;
using BloggerAI.Core.Authorization;
using BloggerAI.Domain;
using BloggerAI.Infrastructure;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#if DEBUG
builder.Configuration.AddJsonFile("appsettings.Development.Local.json", optional: true);
#endif
builder.Configuration.AddEnvironmentVariables();
var authenticationSettigns = new AuthenticationSettings
{
    JwtIssuer = "",
    JwtKey = ""
};
builder.Configuration
    .GetRequiredSection("AuthenticationSettings")
    .Bind(authenticationSettigns);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = authenticationSettigns.JwtIssuer,
            ValidateAudience = true,
            ValidAudience = authenticationSettigns.JwtIssuer,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                authenticationSettigns.JwtKey))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("GetAllPostsPolicy", policy =>
    {
        policy.Requirements.Add(new GetAllPostsAuthoriozationRequirement());
    });
    options.AddPolicy("GetPostByIdPolicy", policy =>
    {
        policy.Requirements.Add(new GetPostByIdRequirement());
    });
    options.AddPolicy("DeletePostByIdPolicy", policy =>
    {
        policy.Requirements.Add(new DeletePostByIdRequirement());
    });
});

builder.Services.AddSingleton(authenticationSettigns);
builder.Services.AddLogging();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var dbConnectionString = builder.Configuration.GetConnectionString("DatabaseConnection");
builder.Services.AddDbContext<BloggerAIDbContext>(options => 
    options.UseSqlServer(dbConnectionString, 
    b => b.MigrationsAssembly("BloggerAI.MSSQL")));
builder.Services.AddHttpContextAccessor()
    .AddBloggerAIServices()
    .AddScoped<IDbContext>(sp => sp.GetRequiredService<BloggerAIDbContext>())
    .AddScoped<DevDataSeeder>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BloggerAIDbContext>();
    await dbContext.Database.MigrateAsync();
    if (app.Environment.IsDevelopment())
    {
        var devDataSeeder = scope.ServiceProvider.GetRequiredService<DevDataSeeder>();
        await devDataSeeder.SeedDevelopmentEnvironmentData();
    }
}

app.Run();

public partial class Program { }