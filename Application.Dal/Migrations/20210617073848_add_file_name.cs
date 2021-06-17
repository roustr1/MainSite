using Microsoft.EntityFrameworkCore.Migrations;

namespace Application.Dal.Migrations
{
    public partial class add_file_name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Files",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { "509907b3-b49c-4a32-95c6-cde2a3cd9ee4", "StoreFilesInDb", "false" },
                    { "74022f9b-9507-4553-816e-5a0dfc0647f0", "Application.Icon", "/images/layout_icons/header.png" },
                    { "120efc54-10fc-45a9-88fe-96e11fc82124", "Application.Name", "" },
                    { "f6da681f-7f95-480c-b021-4a32fd77e42b", "Application.Copy", "" },
                    { "2b6ae5f0-68ad-4746-9665-c73b498f16fc", "BirthdayPath", "http://localhost:50510/api/People/Birthdate?skip=0&take=10" },
                    { "3eaec900-ddea-4e44-a801-f672599cbc5d", "Page.PageSize", "10" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "120efc54-10fc-45a9-88fe-96e11fc82124");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "2b6ae5f0-68ad-4746-9665-c73b498f16fc");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "3eaec900-ddea-4e44-a801-f672599cbc5d");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "509907b3-b49c-4a32-95c6-cde2a3cd9ee4");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "74022f9b-9507-4553-816e-5a0dfc0647f0");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "f6da681f-7f95-480c-b021-4a32fd77e42b");

            migrationBuilder.DropColumn(
                name: "Name",
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
    }
}
