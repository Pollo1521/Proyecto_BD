using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto_BD.Migrations
{
    /// <inheritdoc />
    public partial class horarios2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "JornadaHorario",
                table: "CitasHorario",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JornadaHorario",
                table: "CitasHorario");
        }
    }
}
