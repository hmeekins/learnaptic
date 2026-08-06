using System.ComponentModel.DataAnnotations;

namespace Learnaptic.Api.Dtos.AuthDtos
{
  public class LoginRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;    
    }
}
