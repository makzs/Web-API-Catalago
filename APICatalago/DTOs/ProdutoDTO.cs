using APICatalago.Validations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APICatalago.DTOs
{
    public class ProdutoDTO
    {
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "O nome é obrigatorio")]
        [StringLength(30, ErrorMessage = "O nome deve ter entre 3 e 30 caracteres", MinimumLength = 3)]
        [PrimeiraLetraMaiscula]
        public string? Nome { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "A descrição deve ter no maximo {1} caracteres")]
        public string? Descricao { get; set; }

        [Required]
        [Range(1, 10000, ErrorMessage = "O preco deve estar entre 1 e 10000")]
        public decimal Preco { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 10)]
        public string? ImagemUrl { get; set; }
        public int CategoriaId { get; set; }
    }
}
