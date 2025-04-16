using System.ComponentModel.DataAnnotations;

namespace NetPCAPI.Models
{
    /**
     * <summary>
     * Klasa służąca do opisu tabeli Categories w bazie danych, zawiera ograniczenia i oznaczenie klucza głównego.
     * </summary>
    */
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        // Dane z relacji, nie są częścią tabeli
        public List<Contact> Contacts { get; set; }
        public List<Subcategory> Subcategories { get; set; }
    }
}
