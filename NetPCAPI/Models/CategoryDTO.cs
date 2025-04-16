namespace NetPCAPI.Models
{
    /**
     * <summary>
     * Uproszczona wersja klasy Category, służy do przesyłu danych do frontendu.
     * </summary>
    */
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SubcategoryDto> Subcategories { get; set; }
    }
}
