using JustChatAPI.Data;
using JustChatAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Ajoutez le service de l'Active Directory
builder.Services.AddTransient<ActiveDirectoryService>();

// Ajoutez l'authentification JWT
string secretKey = builder.Configuration["ActiveDirectorySettings:SecretKey"];
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// Ajoutez le service Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Votre API JustChat", Version = "v1" });
});

// Connexion à la base de données PostgreSQL
string connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Votre API JustChat V1");
    });
}

private void DeleteExpiredMessages()
{
    var expiredMessages = _dbContext.Messages.Where(m => m.ExpirationDate < DateTime.UtcNow).ToList();
    _dbContext.Messages.RemoveRange(expiredMessages);
    _dbContext.SaveChanges();
}


var app = builder.Build();
var interval = TimeSpan.FromDays(1); 

Task.Run(async () =>
{
    while (true)
    {
        DeleteExpiredMessages();
        await Task.Delay(interval);
    }
});

app.UseHttpsRedirection();

app.UseAuthentication(); 

app.UseAuthorization();

app.MapControllers();

app.Run();