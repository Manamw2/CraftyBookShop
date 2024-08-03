using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CraftyShop.Migrations
{
    /// <inheritdoc />
    public partial class AddProductsCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CategoryID", "ImageUrl" },
                values: new object[] { 1, "" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CategoryID", "ImageUrl" },
                values: new object[] { 4, "" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CategoryID", "ImageUrl" },
                values: new object[] { 3, "" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CategoryID", "ImageUrl" },
                values: new object[] { 2, "" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CategoryID", "ImageUrl" },
                values: new object[] { 1, "" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CategoryID", "ImageUrl" },
                values: new object[] { 3, "" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryID",
                table: "Products",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryID",
                table: "Products",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Products");
        }
    }
}
