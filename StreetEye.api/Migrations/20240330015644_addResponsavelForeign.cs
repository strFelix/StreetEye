using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StreetEye.Migrations
{
    /// <inheritdoc />
    public partial class addResponsavelForeign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_responsaveis_utilizadores_IdUtilizador",
                table: "responsaveis");

            migrationBuilder.CreateIndex(
                name: "IX_responsaveis_IdResponsavel",
                table: "responsaveis",
                column: "IdResponsavel");

            migrationBuilder.AddForeignKey(
                name: "FK_responsaveis_utilizadores_IdResponsavel",
                table: "responsaveis",
                column: "IdResponsavel",
                principalTable: "utilizadores",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_responsaveis_utilizadores_IdUtilizador",
                table: "responsaveis",
                column: "IdUtilizador",
                principalTable: "utilizadores",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_responsaveis_utilizadores_IdResponsavel",
                table: "responsaveis");

            migrationBuilder.DropForeignKey(
                name: "FK_responsaveis_utilizadores_IdUtilizador",
                table: "responsaveis");

            migrationBuilder.DropIndex(
                name: "IX_responsaveis_IdResponsavel",
                table: "responsaveis");

            migrationBuilder.AddForeignKey(
                name: "FK_responsaveis_utilizadores_IdUtilizador",
                table: "responsaveis",
                column: "IdUtilizador",
                principalTable: "utilizadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
