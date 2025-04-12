using System;
using System.ComponentModel.DataAnnotations;

namespace NetPCAPI.Models
{
    public class Contact
    {
        public int Id { get; set; }
        //[Required]
        public string FirstName { get; set; }
        //[Required]
        public string LastName { get; set; }
        //[Required]
        [EmailAddress]
        public string Email { get; set; }
        //[Required]
        public string Password { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
        //public int CategoryId { get; set; }
        //public Category Category { get; set; }
        //public int? SubcategoryId { get; set; }
        //public Subcategory Subcategory { get; set; }
    }
}