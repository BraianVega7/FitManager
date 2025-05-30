using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class SocioySocioActividadagregados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Socios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dni = table.Column<int>(type: "int", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Localidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Domicilio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Socios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SocioActividad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActividadId = table.Column<int>(type: "int", nullable: false),
                    SocioId = table.Column<int>(type: "int", nullable: false),
                    DiasPorSemana = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocioActividad", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocioActividad_Actividades_ActividadId",
                        column: x => x.ActividadId,
                        principalTable: "Actividades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SocioActividad_Socios_SocioId",
                        column: x => x.SocioId,
                        principalTable: "Socios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SocioActividad_ActividadId",
                table: "SocioActividad",
                column: "ActividadId");

            migrationBuilder.CreateIndex(
                name: "IX_SocioActividad_SocioId",
                table: "SocioActividad",
                column: "SocioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SocioActividad");

            migrationBuilder.DropTable(
                name: "Socios");
        }
    }
}
