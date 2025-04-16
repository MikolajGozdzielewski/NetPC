using System.ComponentModel.DataAnnotations;

namespace NetPCAPI.Models
{
    /**
     * <summary>
     * Klasa służąca do opisu tabeli Subcategories w bazie danych, zawiera ograniczenia i oznaczenie klucza głównego.
     * </summary>
    */
    public class Subcategory
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int CategoryId { get; set; }

        // Dane z relacji, nie są częścią tabeli
        public Category Category { get; set; }
        public List<Contact> Contacts { get; set; }
    }
}
