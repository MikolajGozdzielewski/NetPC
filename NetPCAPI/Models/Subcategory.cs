using System.ComponentModel.DataAnnotations;

namespace NetPCAPI.Models
{
    public class Subcategory
    {
        public int Id { get; set; }
        //[Required]
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<Contact> Contacts { get; set; }
    }
}
