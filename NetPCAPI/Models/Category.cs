using System.ComponentModel.DataAnnotations;

namespace NetPCAPI.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public List<Contact> Contacts { get; set; }
        public List<Subcategory> Subcategories { get; set; }
    }
}
