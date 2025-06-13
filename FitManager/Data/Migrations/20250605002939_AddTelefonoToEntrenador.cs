using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTelefonoToEntrenador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "Entrenadores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "Entrenadores");
        }
    }
}
