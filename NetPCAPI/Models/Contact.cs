using System;
using System.ComponentModel.DataAnnotations;

namespace NetPCAPI.Models
{
    /**
     * <summary>
     * Klasa służąca do opisu tabeli Contacts w bazie danych, zawiera ograniczenia i oznaczenie klucza głównego.
     * </summary>
    */
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public int CategoryId { get; set; }
        public int? SubcategoryId { get; set; }
        public string? AnotherSubcategory { get; set; }

        // Dane z relacji, nie są częścią tabeli
        public Category? Category { get; set; }
        public Subcategory? Subcategory { get; set; }
    }
}