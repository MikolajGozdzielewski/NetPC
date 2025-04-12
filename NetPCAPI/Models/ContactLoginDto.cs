using System.ComponentModel.DataAnnotations;

namespace NetPCAPI.Models
{
    public class ContactLoginDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
