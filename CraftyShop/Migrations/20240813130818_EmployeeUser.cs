using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CraftyShop.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "566f76e3-dc5d-44c9-9138-3c3e111fb8a0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "84898c0e-4fc8-4af2-b3d8-b5308d42a90b");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "10b1b8f4-9c1d-4a3d-b491-7c000bb79da5", "1" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "10b1b8f4-9c1d-4a3d-b491-7c000bb79da5");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "337d453e-0e3d-4449-8bfc-8247cb1ffa86", null, "company", "Company" },
                    { "59d368f2-df47-4ff2-b065-705455da0189", null, "user", "USER" },
                    { "7247d75f-d767-43c2-9c9d-a70aa8f6fa11", null, "employee", "EMPLOYEE" },
                    { "f2696790-dee7-45d9-ab0a-ab31f6817e9e", null, "admin", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fa33300d-4f62-4228-a086-19b5f9f08d20", "AQAAAAIAAYagAAAAEHghXsReUspb8VweQ2NOXxdwhp+zzeYSsrmA5zYNZ6mJZsCyFnf+OGhF5sUazsd58Q==", "c5e3fabe-9b5c-4274-bcc7-6e26ac63c48c" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "f2696790-dee7-45d9-ab0a-ab31f6817e9e", "1" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "337d453e-0e3d-4449-8bfc-8247cb1ffa86");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "59d368f2-df47-4ff2-b065-705455da0189");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7247d75f-d767-43c2-9c9d-a70aa8f6fa11");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "f2696790-dee7-45d9-ab0a-ab31f6817e9e", "1" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f2696790-dee7-45d9-ab0a-ab31f6817e9e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "10b1b8f4-9c1d-4a3d-b491-7c000bb79da5", null, "admin", "ADMIN" },
                    { "566f76e3-dc5d-44c9-9138-3c3e111fb8a0", null, "company", "Company" },
                    { "84898c0e-4fc8-4af2-b3d8-b5308d42a90b", null, "user", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ee8b8860-fd22-49ec-9017-cbeed1b08939", "AQAAAAIAAYagAAAAEKBLqOr1Vw/C4Skpx0rosmsZni9hYsAqL1Phyg/53VLF5IUen/IlWH5I8A90sOy9og==", "787dccf3-1c2a-4861-9298-57b3379485aa" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "10b1b8f4-9c1d-4a3d-b491-7c000bb79da5", "1" });
        }
    }
}
