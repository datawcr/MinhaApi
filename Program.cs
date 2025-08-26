using MinhaApi.Models;
using MinhaApi.Controllers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// logging básico no console
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Configuração do DbContext (aqui InMemory, mas pode ser Postgres)
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("TestDb"));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 👉 Só agora criamos o app
var app = builder.Build();

// exemplo de log
app.MapGet("/", () =>
{
    app.Logger.LogInformation("Hit em / às {time}", DateTime.UtcNow);
    return "API rodando!";
});


// CRUD básico de produtos
app.MapGet("/produtos", async (AppDbContext db, ILogger<Program> logger) =>
{
var lista = await db.Produtos.ToListAsync();
logger.LogInformation("GET /produtos - {qtde} itens", lista.Count);
return Results.Ok(lista);
});

app.MapPost("/produtos", async (Produto p, AppDbContext db, ILogger<Program> logger) =>
{
db.Produtos.Add(p);
await db.SaveChangesAsync();
logger.LogInformation("POST /produtos - criado {id} {nome}", p.Id, p.Nome);
return Results.Created($"/produtos/{p.Id}", p);
});

// Seed de dados InMemory
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Produtos.AddRange(
        new Produto { Nome = "Teste A", Preco = 1m, Quantidade = 1 },
        new Produto { Nome = "Teste B", Preco = 2m, Quantidade = 2 }
    );
    db.SaveChanges();
}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

