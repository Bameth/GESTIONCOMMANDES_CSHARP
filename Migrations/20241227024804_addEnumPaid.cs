using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GESTIONCOMMANDES.Migrations
{
    /// <inheritdoc />
    public partial class addEnumPaid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatutPaiement",
                table: "Commandes",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatutPaiement",
                table: "Commandes");
        }
    }
}
