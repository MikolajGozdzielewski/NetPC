namespace NetPCUI.Models
{
    /**
     * <summary>
     * Klasa służy do odbierania danych z tabeli Contacts przez API.
     * </summary>
    */
    public class ContactDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; } // Dodatkowe pole dla wyświetlania nazwy kategorii
        public int? SubcategoryId { get; set; }
        public string? SubcategoryName { get; set; } // Dodatkowe pole dla wyświetlania nazwy podkategorii
        public string? AnotherSubcategory { get; set; }

    }
}
