using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StreetEye.Migrations
{
    /// <inheritdoc />
    public partial class addDefaultSemaforo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "semaforos",
                columns: new[] { "Id", "Descricao", "Endereco", "IntervaloAberto", "IntervaloFechado", "Latitude", "Longitude", "Numero", "ViaCruzamento" },
                values: new object[] { 1, "Principal", "Rua Alcantara", 20, 40, "-23.519502072656618", "-46.59639509306988", "113", "Guilherme Cotching" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "semaforos",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
