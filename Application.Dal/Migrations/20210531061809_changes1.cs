using Microsoft.EntityFrameworkCore.Migrations;

namespace Application.Dal.Migrations
{
    public partial class changes1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "06af3fe6-12de-447c-9d4d-587e2d591b5a");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "1a515f4c-f9c7-401d-b488-b92c780ffc0e");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "541467c2-d274-449c-8932-a0a979d0752d");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "5f9f2f36-01f9-4fe1-b016-40e270e2b4af");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "c2083da3-c827-486e-b26f-fddbef088e36");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "d3ddea25-7b4f-4ade-9e22-44f82136bddf");

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { "00f43a66-3192-4567-88cd-cb4795f88c6a", "StoreFilesInDb", "false" },
                    { "e8d7054d-0c1b-4180-9a61-eba0ca6d25af", "Application.Icon", "/images/layout_icons/header.png" },
                    { "84333849-ef44-44fb-bc25-e761eb53d0d1", "Application.Name", "" },
                    { "55c44d32-ebaa-44aa-bb9c-4af9db4f47a6", "Application.Copy", "" },
                    { "b43c4a92-142f-42ac-9a7f-a4c6427cfb8b", "BirthdayPath", "http://localhost:50510/api/People/Birthdate?skip=0&take=10" },
                    { "29ba13cc-8944-450e-930e-15f81db4ea4c", "Page.PageSize", "3" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "00f43a66-3192-4567-88cd-cb4795f88c6a");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "29ba13cc-8944-450e-930e-15f81db4ea4c");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "55c44d32-ebaa-44aa-bb9c-4af9db4f47a6");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "84333849-ef44-44fb-bc25-e761eb53d0d1");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "b43c4a92-142f-42ac-9a7f-a4c6427cfb8b");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "e8d7054d-0c1b-4180-9a61-eba0ca6d25af");

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { "c2083da3-c827-486e-b26f-fddbef088e36", "StoreFilesInDb", "false" },
                    { "06af3fe6-12de-447c-9d4d-587e2d591b5a", "Application.Icon", "/images/layout_icons/header.png" },
                    { "d3ddea25-7b4f-4ade-9e22-44f82136bddf", "Application.Name", "" },
                    { "1a515f4c-f9c7-401d-b488-b92c780ffc0e", "Application.Copy", "" },
                    { "541467c2-d274-449c-8932-a0a979d0752d", "BirthdayPath", "http://localhost:50510/api/People/Birthdate?skip=0&take=10" },
                    { "5f9f2f36-01f9-4fe1-b016-40e270e2b4af", "Page.PageSize", "3" }
                });
        }
    }
}
