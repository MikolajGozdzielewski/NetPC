﻿using Microsoft.EntityFrameworkCore;
using NetPCAPI.Models;

namespace NetPCAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfiguracja relacji Subcategory -> Category (jeden-do-wielu)
            modelBuilder.Entity<Subcategory>()
                .HasOne(subcategory => subcategory.Category)
                .WithMany(category => category.Subcategories)
                .HasForeignKey(subcategory => subcategory.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Konfiguracja relacji Contact -> Category (wiele-do-jednego)
            modelBuilder.Entity<Contact>()
                .HasOne(contact => contact.Category)
                .WithMany(category => category.Contacts)
                .HasForeignKey(contact => contact.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Konfiguracja relacji Contact -> Subcategory (wiele-do-jednego)
            modelBuilder.Entity<Contact>()
                .HasOne(contact => contact.Subcategory)
                .WithMany(subcategory => subcategory.Contacts)
                .HasForeignKey(contact => contact.SubcategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}