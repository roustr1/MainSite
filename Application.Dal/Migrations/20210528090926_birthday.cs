using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Application.Dal.Migrations
{
    public partial class birthday : Migration
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

            migrationBuilder.CreateTable(
                name: "birthday_Table",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShortDep = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FIO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Birth = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_birthday_Table", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "birthday_Table");

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
