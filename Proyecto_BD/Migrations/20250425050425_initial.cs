using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto_BD.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Consultorio",
                columns: table => new
                {
                    ID_Consultorio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Piso = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Numero_Consultorio = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultorio", x => x.ID_Consultorio);
                });

            migrationBuilder.CreateTable(
                name: "Especialidad",
                columns: table => new
                {
                    ID_Especialidad = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrecioCita = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Especialidad", x => x.ID_Especialidad);
                });

            migrationBuilder.CreateTable(
                name: "EstatusCita",
                columns: table => new
                {
                    ID_Estatus_Cita = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Estatus_Cita = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstatusCita", x => x.ID_Estatus_Cita);
                });

            migrationBuilder.CreateTable(
                name: "Jornada",
                columns: table => new
                {
                    ID_Jornada = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hora_Entrada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hora_Salida = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jornada", x => x.ID_Jornada);
                });

            migrationBuilder.CreateTable(
                name: "Medicina",
                columns: table => new
                {
                    ID_Medicina = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Precio_Medicina = table.Column<float>(type: "real", nullable: false),
                    Nombre_Medicina = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicina", x => x.ID_Medicina);
                });

            migrationBuilder.CreateTable(
                name: "Servicio",
                columns: table => new
                {
                    ID_Servicio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Precio_Servicio = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicio", x => x.ID_Servicio);
                });

            migrationBuilder.CreateTable(
                name: "TipoSangre",
                columns: table => new
                {
                    ID_Tipo_Sangre = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo_Sangre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoSangre", x => x.ID_Tipo_Sangre);
                });

            migrationBuilder.CreateTable(
                name: "TiposUsuario",
                columns: table => new
                {
                    ID_Tipo_Usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo_Usuario = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposUsuario", x => x.ID_Tipo_Usuario);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    ID_Usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido_Paterno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido_Materno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CURP = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false),
                    Fecha_Nacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha_Registro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ID_Tipo_Usuario = table.Column<int>(type: "int", nullable: false),
                    Estado_Usuario = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.ID_Usuario);
                    table.ForeignKey(
                        name: "FK_Usuario_TiposUsuario_ID_Tipo_Usuario",
                        column: x => x.ID_Tipo_Usuario,
                        principalTable: "TiposUsuario",
                        principalColumn: "ID_Tipo_Usuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Medico",
                columns: table => new
                {
                    ID_Medico = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Usuario = table.Column<int>(type: "int", nullable: false),
                    Cedula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ID_Especialidad = table.Column<int>(type: "int", nullable: false),
                    ID_Consultorio = table.Column<int>(type: "int", nullable: false),
                    ID_Jornada = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medico", x => x.ID_Medico);
                    table.ForeignKey(
                        name: "FK_Medico_Consultorio_ID_Consultorio",
                        column: x => x.ID_Consultorio,
                        principalTable: "Consultorio",
                        principalColumn: "ID_Consultorio",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Medico_Especialidad_ID_Especialidad",
                        column: x => x.ID_Especialidad,
                        principalTable: "Especialidad",
                        principalColumn: "ID_Especialidad",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Medico_Jornada_ID_Jornada",
                        column: x => x.ID_Jornada,
                        principalTable: "Jornada",
                        principalColumn: "ID_Jornada",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Medico_Usuario_ID_Usuario",
                        column: x => x.ID_Usuario,
                        principalTable: "Usuario",
                        principalColumn: "ID_Usuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Paciente",
                columns: table => new
                {
                    ID_Paciente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Usuario = table.Column<int>(type: "int", nullable: false),
                    ID_Tipo_Sangre = table.Column<int>(type: "int", nullable: false),
                    Peso = table.Column<float>(type: "real", nullable: false),
                    Alergia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estatura = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paciente", x => x.ID_Paciente);
                    table.ForeignKey(
                        name: "FK_Paciente_TipoSangre_ID_Tipo_Sangre",
                        column: x => x.ID_Tipo_Sangre,
                        principalTable: "TipoSangre",
                        principalColumn: "ID_Tipo_Sangre",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Paciente_Usuario_ID_Usuario",
                        column: x => x.ID_Usuario,
                        principalTable: "Usuario",
                        principalColumn: "ID_Usuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Recepcionista",
                columns: table => new
                {
                    ID_Recepcionista = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Usuario = table.Column<int>(type: "int", nullable: false),
                    ID_Jornada = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recepcionista", x => x.ID_Recepcionista);
                    table.ForeignKey(
                        name: "FK_Recepcionista_Jornada_ID_Jornada",
                        column: x => x.ID_Jornada,
                        principalTable: "Jornada",
                        principalColumn: "ID_Jornada",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Recepcionista_Usuario_ID_Usuario",
                        column: x => x.ID_Usuario,
                        principalTable: "Usuario",
                        principalColumn: "ID_Usuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cita",
                columns: table => new
                {
                    ID_Cita = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Paciente = table.Column<int>(type: "int", nullable: false),
                    ID_Medico = table.Column<int>(type: "int", nullable: false),
                    Fecha_Registro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fecha_Cita = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hora_Cita = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ID_Estatus_Cita = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cita", x => x.ID_Cita);
                    table.ForeignKey(
                        name: "FK_Cita_EstatusCita_ID_Estatus_Cita",
                        column: x => x.ID_Estatus_Cita,
                        principalTable: "EstatusCita",
                        principalColumn: "ID_Estatus_Cita",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cita_Medico_ID_Medico",
                        column: x => x.ID_Medico,
                        principalTable: "Medico",
                        principalColumn: "ID_Medico",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cita_Paciente_ID_Paciente",
                        column: x => x.ID_Paciente,
                        principalTable: "Paciente",
                        principalColumn: "ID_Paciente",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Venta",
                columns: table => new
                {
                    ID_Ventas = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha_Venta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ID_Recepcionista = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venta", x => x.ID_Ventas);
                    table.ForeignKey(
                        name: "FK_Venta_Recepcionista_ID_Recepcionista",
                        column: x => x.ID_Recepcionista,
                        principalTable: "Recepcionista",
                        principalColumn: "ID_Recepcionista",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pago",
                columns: table => new
                {
                    ID_Pago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Cita = table.Column<int>(type: "int", nullable: false),
                    Estado_Pago = table.Column<bool>(type: "bit", nullable: false),
                    ComprobantePago = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pago", x => x.ID_Pago);
                    table.ForeignKey(
                        name: "FK_Pago_Cita_ID_Cita",
                        column: x => x.ID_Cita,
                        principalTable: "Cita",
                        principalColumn: "ID_Cita",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Receta",
                columns: table => new
                {
                    ID_Receta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Cita = table.Column<int>(type: "int", nullable: false),
                    Diagnostico = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receta", x => x.ID_Receta);
                    table.ForeignKey(
                        name: "FK_Receta_Cita_ID_Cita",
                        column: x => x.ID_Cita,
                        principalTable: "Cita",
                        principalColumn: "ID_Cita",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    ID_Ticket = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Venta = table.Column<int>(type: "int", nullable: false),
                    ID_Medicina = table.Column<int>(type: "int", nullable: false),
                    ID_Servicio = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.ID_Ticket);
                    table.ForeignKey(
                        name: "FK_Ticket_Medicina_ID_Medicina",
                        column: x => x.ID_Medicina,
                        principalTable: "Medicina",
                        principalColumn: "ID_Medicina",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ticket_Servicio_ID_Servicio",
                        column: x => x.ID_Servicio,
                        principalTable: "Servicio",
                        principalColumn: "ID_Servicio",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ticket_Venta_ID_Venta",
                        column: x => x.ID_Venta,
                        principalTable: "Venta",
                        principalColumn: "ID_Ventas",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tratamiento",
                columns: table => new
                {
                    ID_Tratamiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Receta = table.Column<int>(type: "int", nullable: false),
                    Medicamento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Indicaciones = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tratamiento", x => x.ID_Tratamiento);
                    table.ForeignKey(
                        name: "FK_Tratamiento_Receta_ID_Receta",
                        column: x => x.ID_Receta,
                        principalTable: "Receta",
                        principalColumn: "ID_Receta",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cita_ID_Estatus_Cita",
                table: "Cita",
                column: "ID_Estatus_Cita");

            migrationBuilder.CreateIndex(
                name: "IX_Cita_ID_Medico",
                table: "Cita",
                column: "ID_Medico");

            migrationBuilder.CreateIndex(
                name: "IX_Cita_ID_Paciente",
                table: "Cita",
                column: "ID_Paciente");

            migrationBuilder.CreateIndex(
                name: "IX_Medico_ID_Consultorio",
                table: "Medico",
                column: "ID_Consultorio");

            migrationBuilder.CreateIndex(
                name: "IX_Medico_ID_Especialidad",
                table: "Medico",
                column: "ID_Especialidad");

            migrationBuilder.CreateIndex(
                name: "IX_Medico_ID_Jornada",
                table: "Medico",
                column: "ID_Jornada");

            migrationBuilder.CreateIndex(
                name: "IX_Medico_ID_Usuario",
                table: "Medico",
                column: "ID_Usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Paciente_ID_Tipo_Sangre",
                table: "Paciente",
                column: "ID_Tipo_Sangre");

            migrationBuilder.CreateIndex(
                name: "IX_Paciente_ID_Usuario",
                table: "Paciente",
                column: "ID_Usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Pago_ID_Cita",
                table: "Pago",
                column: "ID_Cita");

            migrationBuilder.CreateIndex(
                name: "IX_Recepcionista_ID_Jornada",
                table: "Recepcionista",
                column: "ID_Jornada");

            migrationBuilder.CreateIndex(
                name: "IX_Recepcionista_ID_Usuario",
                table: "Recepcionista",
                column: "ID_Usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Receta_ID_Cita",
                table: "Receta",
                column: "ID_Cita");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_ID_Medicina",
                table: "Ticket",
                column: "ID_Medicina");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_ID_Servicio",
                table: "Ticket",
                column: "ID_Servicio");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_ID_Venta",
                table: "Ticket",
                column: "ID_Venta");

            migrationBuilder.CreateIndex(
                name: "IX_Tratamiento_ID_Receta",
                table: "Tratamiento",
                column: "ID_Receta");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_ID_Tipo_Usuario",
                table: "Usuario",
                column: "ID_Tipo_Usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Venta_ID_Recepcionista",
                table: "Venta",
                column: "ID_Recepcionista");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pago");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "Tratamiento");

            migrationBuilder.DropTable(
                name: "Medicina");

            migrationBuilder.DropTable(
                name: "Servicio");

            migrationBuilder.DropTable(
                name: "Venta");

            migrationBuilder.DropTable(
                name: "Receta");

            migrationBuilder.DropTable(
                name: "Recepcionista");

            migrationBuilder.DropTable(
                name: "Cita");

            migrationBuilder.DropTable(
                name: "EstatusCita");

            migrationBuilder.DropTable(
                name: "Medico");

            migrationBuilder.DropTable(
                name: "Paciente");

            migrationBuilder.DropTable(
                name: "Consultorio");

            migrationBuilder.DropTable(
                name: "Especialidad");

            migrationBuilder.DropTable(
                name: "Jornada");

            migrationBuilder.DropTable(
                name: "TipoSangre");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "TiposUsuario");
        }
    }
}
