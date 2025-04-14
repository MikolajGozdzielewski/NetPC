using System;
using System.ComponentModel.DataAnnotations;

namespace NetPCAPI.Models
{
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
        
        public Category? Category { get; set; }
        public Subcategory? Subcategory { get; set; }
    }
}