using System.ComponentModel.DataAnnotations;

namespace NetPCAPI.Models
{
    /**
     * <summary>
     * Klasa odbierająca dane podczas logowania.
     * </summary>
    */
    public class ContactLoginDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
