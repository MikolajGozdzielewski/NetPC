namespace NetPCUI.Models
{
    /**
     * <summary>
     * Klasa służy do odbierania danych z tabeli Subcategory przez API.
     * </summary>
    */
    public class SubcategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
    }
}
