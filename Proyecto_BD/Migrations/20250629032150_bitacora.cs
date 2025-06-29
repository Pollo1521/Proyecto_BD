using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto_BD.Migrations
{
    /// <inheritdoc />
    public partial class bitacora : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bitacoras",
                columns: table => new
                {
                    ID_Bitacora = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha_Movimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ID_Medico = table.Column<int>(type: "int", nullable: false),
                    ID_Paciente = table.Column<int>(type: "int", nullable: false),
                    ID_Cita = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bitacoras", x => x.ID_Bitacora);
                    table.ForeignKey(
                        name: "FK_Bitacoras_Cita_ID_Cita",
                        column: x => x.ID_Cita,
                        principalTable: "Cita",
                        principalColumn: "ID_Cita",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bitacoras_Medico_ID_Medico",
                        column: x => x.ID_Medico,
                        principalTable: "Medico",
                        principalColumn: "ID_Medico",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bitacoras_Paciente_ID_Paciente",
                        column: x => x.ID_Paciente,
                        principalTable: "Paciente",
                        principalColumn: "ID_Paciente",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bitacoras_ID_Cita",
                table: "Bitacoras",
                column: "ID_Cita");

            migrationBuilder.CreateIndex(
                name: "IX_Bitacoras_ID_Medico",
                table: "Bitacoras",
                column: "ID_Medico");

            migrationBuilder.CreateIndex(
                name: "IX_Bitacoras_ID_Paciente",
                table: "Bitacoras",
                column: "ID_Paciente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bitacoras");
        }
    }
}
