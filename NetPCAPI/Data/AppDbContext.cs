using Microsoft.EntityFrameworkCore;
using NetPCAPI.Models;

namespace NetPCAPI.Data
{
    /**
     * <summary>
     * Funkcja opisuję bazę danych na podstawie klas i relacji.
     * </summary>
    */
    public class AppDbContext : DbContext
    {
        // Wybranie klas na podstawie, których zostanie stworzona baza danych.
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Wprowadzenie relacji między tabelami
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relacja Subcategory - Category (wiele do jednego)
            modelBuilder.Entity<Subcategory>()
                .HasOne(subcategory => subcategory.Category)
                .WithMany(category => category.Subcategories)
                .HasForeignKey(subcategory => subcategory.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacja Contact -> Category (wiele do jednego)
            modelBuilder.Entity<Contact>()
                .HasOne(contact => contact.Category)
                .WithMany(category => category.Contacts)
                .HasForeignKey(contact => contact.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacja Contact -> Subcategory (wiele-do-jednego)
            modelBuilder.Entity<Contact>()
                .HasOne(contact => contact.Subcategory)
                .WithMany(subcategory => subcategory.Contacts)
                .HasForeignKey(contact => contact.SubcategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}