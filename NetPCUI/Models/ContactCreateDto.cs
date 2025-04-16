﻿using System.ComponentModel.DataAnnotations;

namespace NetPCUI.Models
{
    public class ContactCreateDto
    {
        [Required(ErrorMessage = "Imię jest wymagane")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email jest wymagany")]
        [EmailAddress(ErrorMessage = "Podaj poprawny adres email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Hasło musi mieć co najmniej 8 znaków")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Numer telefonu jest wymagany")]
        [Phone(ErrorMessage = "Podaj poprawny numer telefonu")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Data urodzenia jest wymagania")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "ID kategorii jest wymagane")]
        public int CategoryId { get; set; }
        public int? SubcategoryId { get; set; }
        public string? AnotherSubcategory { get; set; }

    }
}
