using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto_BD.Migrations
{
    /// <inheritdoc />
    public partial class Consultorios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Disponible",
                table: "Consultorio",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Disponible",
                table: "Consultorio");
        }
    }
}
