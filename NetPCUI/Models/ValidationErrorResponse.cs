namespace NetPCUI.Models
{
    /**
     * <summary>
     * Klasa reprezentuje błąd walidacji, wykorzystywana przy dodawaniu kontaktu, w ContactService.cs 
     * </summary>
    */
    public class ValidationErrorResponse
    {
        public Dictionary<string, List<string>> Errors { get; set; }
    }
}
