using System.ComponentModel.DataAnnotations;

namespace Learnaptic.Api.Features.Auth.Dtos
{
    public class RegisterRequestDto
    {
        [Required]
        [StringLength(40, MinimumLength = 4)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
    }
}
