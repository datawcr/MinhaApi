using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MinhaApi.Models
{
public class Produto
{
[Key]
public int Id { get; set; }
[Required]
[StringLength(120)]
public string Nome { get; set; } = string.Empty;
[StringLength(300)]
public string? Descricao { get; set; }
[Column(TypeName = "numeric(10,2)")]
[Range(0, 999999999.99)]
public decimal Preco { get; set; }
public int? Estoque { get; set; }
public int Quantidade { get; set; }
}
}
