

using APICatalago.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalago.Models
{
    [Table("Produto")]
    public class Produto
    {
        // utilizando validações usando data anotations
        [Key]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage ="O nome é obrigatorio")]
        [StringLength(30, ErrorMessage ="O nome deve ter entre 3 e 30 caracteres", MinimumLength = 3)]
        [PrimeiraLetraMaiscula]
        public string? Nome { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "A descrição deve ter no maximo {1} caracteres")]
        public string? Descricao { get; set; }

        [Required]
        [Column(TypeName ="decimal(10,2)")]
        [Range(1, 10000, ErrorMessage ="O preco deve estar entre 1 e 10000")]
        public decimal Preco { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 10)]
        public string? ImagemUrl { get; set; }
        public float Estoque { get; set; }
        public DateTime DataCadastro { get; set; }
        public int CategoriaId { get; set; }

        [JsonIgnore]
        public Categoria? Categoria { get; set; }

        
    }
}
