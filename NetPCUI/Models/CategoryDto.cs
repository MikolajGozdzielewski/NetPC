namespace NetPCUI.Models
{
    /**
     * <summary>
     * Klasa służy do odbierania danych z tabeli Category przez API.
     * </summary>
    */
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
