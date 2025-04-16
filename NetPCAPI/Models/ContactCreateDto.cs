using System.ComponentModel.DataAnnotations;

namespace NetPCAPI.Models
{
    public class ContactCreateDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Nieprawidłowy adres email.")]
        public string Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Hasło musi mieć co najmniej 8 znaków.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).+$", ErrorMessage = "Hasło musi zawierać litery i cyfry.")]
        public string Password { get; set; }

        [RegularExpression(@"^\d{9,}$", ErrorMessage = "Numer telefonu musi zawierać co najmniej 9 cyfr.")]
        public string? PhoneNumber { get; set; }

        public DateTime? BirthDate { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public int? SubcategoryId { get; set; }
        public string? AnotherSubcategory { get; set; }
    }
}
