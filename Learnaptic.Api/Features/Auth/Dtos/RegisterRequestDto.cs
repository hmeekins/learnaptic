using System.ComponentModel.DataAnnotations;

namespace Learnaptic.Api.Features.Auth.Dtos
{
    public class RegisterRequestDto
    {
        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 10)]
        public string Password { get; set; } = string.Empty;
    }
}
