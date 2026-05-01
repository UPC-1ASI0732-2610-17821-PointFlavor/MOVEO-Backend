using Microsoft.EntityFrameworkCore;
using PuntoSabor_Backend.Auth.Domain.Repositories;
using PuntoSabor_Backend.Auth.Infrastructure.Persistence.EFC.Repositories;
using PuntoSabor_Backend.Discovery.Domain.Repositories;
using PuntoSabor_Backend.Discovery.Infrastructure.Persistence.EFC.Repositories;
using PuntoSabor_Backend.Memberships.Domain.Repositories;
using PuntoSabor_Backend.Memberships.Infrastructure.Persistence.EFC.Repositories;
using PuntoSabor_Backend.Promotions.Domain.Repositories;
using PuntoSabor_Backend.Promotions.Infrastructure.Persistence.EFC.Repositories;
using PuntoSabor_Backend.Reviews.Domain.Repositories;
using PuntoSabor_Backend.Reviews.Infrastructure.Persistence.EFC.Repositories;
using PuntoSabor_Backend.Shared.Domain.Repositories;
using PuntoSabor_Backend.Shared.Infrastructure.Persistence.EFC;

var builder = WebApplication.CreateBuilder(args);

// Debug: variables Railway
Console.WriteLine(">>> ENV DB_HOST: " + Environment.GetEnvironmentVariable("DB_HOST"));
Console.WriteLine(">>> ENV DB_PORT: " + Environment.GetEnvironmentVariable("DB_PORT"));
Console.WriteLine(">>> ENV DB_NAME: " + Environment.GetEnvironmentVariable("DB_NAME"));
Console.WriteLine(">>> ENV DB_USER: " + Environment.GetEnvironmentVariable("DB_USER"));
Console.WriteLine(">>> ENV DB_PASSWORD: " + Environment.GetEnvironmentVariable("DB_PASSWORD"));

// Build connection string
var connectionString =
    $"server={Environment.GetEnvironmentVariable("DB_HOST")};" +
    $"port={Environment.GetEnvironmentVariable("DB_PORT")};" +
    $"database={Environment.GetEnvironmentVariable("DB_NAME")};" +
    $"user={Environment.GetEnvironmentVariable("DB_USER")};" +
    $"password={Environment.GetEnvironmentVariable("DB_PASSWORD")}";

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySQL(connectionString!);
});

// Unit of Work
builder.Services.AddScoped<IUnitOfWork, AppUnitOfWork>();

// Repositories
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IHuariqueRepository, HuariqueRepository>();
builder.Services.AddScoped<IPlanRepository, PlanRepository>();
builder.Services.AddScoped<IPromoRepository, PromoRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Controllers + JSON
builder.Services.AddControllers().AddNewtonsoftJson();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

const string corsPolicyName = "PuntoSaborCors";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
                "https://puntosabor.netlify.app",
                "https://frontend-punto-sabor.vercel.app"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        Console.WriteLine(">>> Verificando base de datos...");
        db.Database.EnsureCreated();

        Console.WriteLine(">>> Ejecutando DataSeeder...");
        DataSeeder.Seed(db);
        Console.WriteLine(">>> DataSeeder terminado.");
    }
    catch (Exception ex)
    {
        Console.WriteLine(">>> ERROR AL CONECTAR A MYSQL EN STARTUP:");
        Console.WriteLine(ex.Message);
    }
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors(corsPolicyName);
app.UseAuthorization();
app.MapControllers();
app.Run();
