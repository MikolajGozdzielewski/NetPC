using System.ComponentModel.DataAnnotations;

namespace NetPCAPI.Models
{
    public class Subcategory
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<Contact> Contacts { get; set; }
    }
}
