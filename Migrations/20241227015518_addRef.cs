using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GESTIONCOMMANDES.Migrations
{
    /// <inheritdoc />
    public partial class addRef : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "Paiements",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reference",
                table: "Paiements");
        }
    }
}
