using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto_BD.Migrations
{
    /// <inheritdoc />
    public partial class horarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hora_Cita",
                table: "Cita");

            migrationBuilder.AddColumn<int>(
                name: "ID_Cita_Horario",
                table: "Cita",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CitasHorario",
                columns: table => new
                {
                    ID_Horario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hora_Cita = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitasHorario", x => x.ID_Horario);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cita_ID_Cita_Horario",
                table: "Cita",
                column: "ID_Cita_Horario");

            migrationBuilder.AddForeignKey(
                name: "FK_Cita_CitasHorario_ID_Cita_Horario",
                table: "Cita",
                column: "ID_Cita_Horario",
                principalTable: "CitasHorario",
                principalColumn: "ID_Horario",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cita_CitasHorario_ID_Cita_Horario",
                table: "Cita");

            migrationBuilder.DropTable(
                name: "CitasHorario");

            migrationBuilder.DropIndex(
                name: "IX_Cita_ID_Cita_Horario",
                table: "Cita");

            migrationBuilder.DropColumn(
                name: "ID_Cita_Horario",
                table: "Cita");

            migrationBuilder.AddColumn<DateTime>(
                name: "Hora_Cita",
                table: "Cita",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
