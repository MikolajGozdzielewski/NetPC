namespace NetPCAPI.Models
{
    /**
     * <summary>
     * Uproszczona wersja klasy Contact, służy do przesyłu danych do frontendu, nie zawiera hasła.
     * </summary>
    */
    public class ContactDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? SubcategoryId { get; set; }
        public string? SubcategoryName { get; set; }
        public string? AnotherSubcategory { get; set; }
    }
}
