using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto_BD.Migrations
{
    /// <inheritdoc />
    public partial class tickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "Recibo");

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    ID_Ticket = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Venta = table.Column<int>(type: "int", nullable: false),
                    Tipo_item = table.Column<bool>(type: "bit", nullable: false),
                    ID_Item = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.ID_Ticket);
                    table.ForeignKey(
                        name: "FK_Ticket_Venta_ID_Venta",
                        column: x => x.ID_Venta,
                        principalTable: "Venta",
                        principalColumn: "ID_Ventas",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_ID_Venta",
                table: "Ticket",
                column: "ID_Venta");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.CreateTable(
                name: "Recibo",
                columns: table => new
                {
                    ID_Recibo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Venta = table.Column<int>(type: "int", nullable: false),
                    ID_Item = table.Column<int>(type: "int", nullable: false),
                    Tipo_item = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recibo", x => x.ID_Recibo);
                    table.ForeignKey(
                        name: "FK_Recibo_Venta_ID_Venta",
                        column: x => x.ID_Venta,
                        principalTable: "Venta",
                        principalColumn: "ID_Ventas",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recibo_ID_Venta",
                table: "Recibo",
                column: "ID_Venta");
        }
    }
}
