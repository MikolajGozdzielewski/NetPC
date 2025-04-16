using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetPCAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeContacts3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AnotherSubcategory",
                table: "Contacts",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnotherSubcategory",
                table: "Contacts");
        }
    }
}
