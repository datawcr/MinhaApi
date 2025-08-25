using Microsoft.AspNetCore.Mvc;
using MinhaApi.Models;
using System.Collections.Generic;
using System.Linq;
namespace MinhaApi.Controllers
{
[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
private readonly AppDbContext _context;
public ProdutosController(AppDbContext context)
{
_context = context;
}
[HttpGet]
public ActionResult<IEnumerable<Produto>> Get()
{
return _context.Produtos.ToList();
}
[HttpPost]
public ActionResult<Produto> Post(Produto produto)
{
_context.Produtos.Add(produto);
_context.SaveChanges();
return CreatedAtAction(nameof(Get), new { id = produto.Id }, produto);
}
}
}
