using Microsoft.EntityFrameworkCore.Migrations;

namespace Application.Dal.Migrations
{
    public partial class updatedatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "5de99529-65e7-4f15-9647-701c7d972f8a");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "6500ebab-11f1-434a-a6e2-20e4c4e068f9");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "a9d9e5ec-f191-4056-aa8c-be72e7e2be5c");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "c5bccc32-130d-45e5-bb7d-9d9447e53d6f");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "e86ae526-f67c-44c4-ab12-b21038562aef");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "ea1d4ac5-1c38-4d72-89c2-0c21d59c9f40");

            migrationBuilder.RenameColumn(
                name: "DepartmentShortName",
                table: "birthday_Table",
                newName: "ShortDep");

            migrationBuilder.RenameColumn(
                name: "DepartmentFullName",
                table: "birthday_Table",
                newName: "Department");

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { "be7cbdd9-2a0d-4227-8e63-f5700971db10", "StoreFilesInDb", "false" },
                    { "65bc3059-bb4b-4a99-8ece-3deb4ddd19d6", "Application.Icon", "/images/layout_icons/header.png" },
                    { "3b6ebd12-c62d-4923-8b42-3837e6031aec", "Application.Name", "" },
                    { "59c52c21-9481-4349-8625-8b4226fc9d69", "Application.Copy", "" },
                    { "cc693194-0ec8-40c5-997a-7b1affc991d0", "BirthdayPath", "http://localhost:50510/api/People/Birthdate?skip=0&take=10" },
                    { "38c7da7b-cb0e-4b2f-8b93-dc5d17aca36f", "Page.PageSize", "3" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "38c7da7b-cb0e-4b2f-8b93-dc5d17aca36f");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "3b6ebd12-c62d-4923-8b42-3837e6031aec");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "59c52c21-9481-4349-8625-8b4226fc9d69");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "65bc3059-bb4b-4a99-8ece-3deb4ddd19d6");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "be7cbdd9-2a0d-4227-8e63-f5700971db10");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "cc693194-0ec8-40c5-997a-7b1affc991d0");

            migrationBuilder.RenameColumn(
                name: "ShortDep",
                table: "birthday_Table",
                newName: "DepartmentShortName");

            migrationBuilder.RenameColumn(
                name: "Department",
                table: "birthday_Table",
                newName: "DepartmentFullName");

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { "ea1d4ac5-1c38-4d72-89c2-0c21d59c9f40", "StoreFilesInDb", "false" },
                    { "6500ebab-11f1-434a-a6e2-20e4c4e068f9", "Application.Icon", "/images/layout_icons/header.png" },
                    { "c5bccc32-130d-45e5-bb7d-9d9447e53d6f", "Application.Name", "" },
                    { "5de99529-65e7-4f15-9647-701c7d972f8a", "Application.Copy", "" },
                    { "e86ae526-f67c-44c4-ab12-b21038562aef", "BirthdayPath", "http://localhost:50510/api/People/Birthdate?skip=0&take=10" },
                    { "a9d9e5ec-f191-4056-aa8c-be72e7e2be5c", "Page.PageSize", "3" }
                });
        }
    }
}
