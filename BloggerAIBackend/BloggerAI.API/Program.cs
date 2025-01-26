
using BloggerAI.Infrastructure;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var dbConnectionString = builder.Configuration.GetConnectionString("DatabaseConnection");
builder.Services.AddDbContext<BloggerAIDbContext>(options => 
    options.UseSqlServer(dbConnectionString, 
    b => b.MigrationsAssembly("BloggerAI.MSSQL")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

using (var scope = app.Services.CreateScope())
using (var dbContext = scope.ServiceProvider.GetRequiredService<BloggerAIDbContext>())
{
    await dbContext.Database.MigrateAsync();
}

app.Run();