using System.ComponentModel.DataAnnotations;

namespace NetPCUI.Models
{
    /**
     * <summary>
     * Klasa służąca do wysyłania danych od frontendu podczas edycji kontaktu, posiada ograniczenia, które będą wykorzystane do walidacji.
     * Różni się od ContactCreateDto, tym że nie ma miejsca na hasło.
     * </summary>
    */
    public class ContactUpdateDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Imię jest wymagane")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email jest wymagany")]
        [EmailAddress(ErrorMessage = "Podaj poprawny adres email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Numer telefonu jest wymagany")]
        [RegularExpression(@"^\d{9,}$", ErrorMessage = "Numer telefonu musi zawierać co najmniej 9 cyfr.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Data urodzenia jest wymagania")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "ID kategorii jest wymagane")]
        public int CategoryId { get; set; }
        public int? SubcategoryId { get; set; }
        public string? AnotherSubcategory { get; set; }

    }
}
