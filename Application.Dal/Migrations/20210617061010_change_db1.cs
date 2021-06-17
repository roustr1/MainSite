using Microsoft.EntityFrameworkCore.Migrations;

namespace Application.Dal.Migrations
{
    public partial class change_db1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "2ce8ec97-5549-404f-9aa0-d9e3e3592693");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "39bd19a8-99d3-45f2-8b6c-98de40e6e92e");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "9093226f-7ec5-4710-bfef-fe582c14e365");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "944341b0-7ab5-41f9-a33f-fb329c676ab2");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "c0618b8b-478b-4521-b2a3-e466e0e89c8d");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "dc66e975-2c7b-42ae-b845-402f1974038a");

            migrationBuilder.DropColumn(
                name: "UseDownloadUrl",
                table: "Files");

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { "6e1ef971-b98c-4982-bb9c-b0e87cab55aa", "StoreFilesInDb", "false" },
                    { "376abf77-0069-4b59-a82a-d0ae6bc32d61", "Application.Icon", "/images/layout_icons/header.png" },
                    { "0e50433f-bda1-43ec-b0f9-b0764eb05967", "Application.Name", "" },
                    { "13212374-1abc-4336-8e81-998f701a595d", "Application.Copy", "" },
                    { "51cbf7a1-479f-4439-af32-11a9c500c8e8", "BirthdayPath", "http://localhost:50510/api/People/Birthdate?skip=0&take=10" },
                    { "7ea74f12-5108-4c1e-b462-53318b3a44fe", "Page.PageSize", "3" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "0e50433f-bda1-43ec-b0f9-b0764eb05967");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "13212374-1abc-4336-8e81-998f701a595d");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "376abf77-0069-4b59-a82a-d0ae6bc32d61");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "51cbf7a1-479f-4439-af32-11a9c500c8e8");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "6e1ef971-b98c-4982-bb9c-b0e87cab55aa");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "7ea74f12-5108-4c1e-b462-53318b3a44fe");

            migrationBuilder.AddColumn<bool>(
                name: "UseDownloadUrl",
                table: "Files",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { "944341b0-7ab5-41f9-a33f-fb329c676ab2", "StoreFilesInDb", "false" },
                    { "9093226f-7ec5-4710-bfef-fe582c14e365", "Application.Icon", "/images/layout_icons/header.png" },
                    { "c0618b8b-478b-4521-b2a3-e466e0e89c8d", "Application.Name", "" },
                    { "2ce8ec97-5549-404f-9aa0-d9e3e3592693", "Application.Copy", "" },
                    { "dc66e975-2c7b-42ae-b845-402f1974038a", "BirthdayPath", "http://localhost:50510/api/People/Birthdate?skip=0&take=10" },
                    { "39bd19a8-99d3-45f2-8b6c-98de40e6e92e", "Page.PageSize", "3" }
                });
        }
    }
}
