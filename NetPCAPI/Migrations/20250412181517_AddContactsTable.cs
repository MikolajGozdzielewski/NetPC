using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetPCAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddContactsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Contacts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Subcategory",
                table: "Contacts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "Subcategory",
                table: "Contacts");
        }
    }
}
