using System.ComponentModel.DataAnnotations;

namespace NetPCAPI.Models
{
    public class ContactUpdateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Nieprawidłowy adres email.")]
        public string Email { get; set; }

        [RegularExpression(@"^\d{9,}$", ErrorMessage = "Numer telefonu musi zawierać co najmniej 9 cyfr.")]
        public string? PhoneNumber { get; set; }

        public DateTime? BirthDate { get; set; }

        public int? CategoryId { get; set; }

        public int? SubcategoryId { get; set; }
    }
}
