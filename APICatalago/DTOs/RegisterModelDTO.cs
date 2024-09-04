using System.ComponentModel.DataAnnotations;

namespace APICatalago.DTOs
{
    public class RegisterModelDTO
    {
        [Required(ErrorMessage = "O nome é obrigatorio")]
        public string? UserName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatoria")]
        public string? Password { get; set; }
    }
}
