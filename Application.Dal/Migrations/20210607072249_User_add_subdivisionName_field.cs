using Microsoft.EntityFrameworkCore.Migrations;

namespace Application.Dal.Migrations
{
    public partial class User_add_subdivisionName_field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "34d17388-e836-433f-9127-92d0d5b2be1a");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "5b6153b7-3eb2-4b93-9a13-4331fe6dafff");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "765a96ea-ffde-4ff2-8ed6-b4d014e6f92f");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "c415d0b2-b5a5-4a5f-93cb-9a3320c7f6c2");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "c96e411e-0b3e-4413-bff6-38e09d5d9cba");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "f6bf6e19-4a3a-4bd0-8506-da6055cf1774");

            migrationBuilder.AddColumn<string>(
                name: "SubdivisionName",
                table: "UserInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { "e3bb226a-dd8b-416e-9ce8-4882f82fa3a6", "StoreFilesInDb", "false" },
                    { "56be5994-088a-40b1-9f29-bc15b79fdc2e", "Application.Icon", "/images/layout_icons/header.png" },
                    { "7d32f01d-d679-4229-807e-d281d7cecd9f", "Application.Name", "" },
                    { "d809a722-8144-421d-b6b2-89b28c761532", "Application.Copy", "" },
                    { "c20efe04-62e1-4344-a626-623bf68e8a62", "BirthdayPath", "http://localhost:50510/api/People/Birthdate?skip=0&take=10" },
                    { "d6acc530-acab-494a-a592-0a95d2aeefae", "Page.PageSize", "3" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "56be5994-088a-40b1-9f29-bc15b79fdc2e");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "7d32f01d-d679-4229-807e-d281d7cecd9f");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "c20efe04-62e1-4344-a626-623bf68e8a62");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "d6acc530-acab-494a-a592-0a95d2aeefae");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "d809a722-8144-421d-b6b2-89b28c761532");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: "e3bb226a-dd8b-416e-9ce8-4882f82fa3a6");

            migrationBuilder.DropColumn(
                name: "SubdivisionName",
                table: "UserInfo");

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { "765a96ea-ffde-4ff2-8ed6-b4d014e6f92f", "StoreFilesInDb", "false" },
                    { "c415d0b2-b5a5-4a5f-93cb-9a3320c7f6c2", "Application.Icon", "/images/layout_icons/header.png" },
                    { "34d17388-e836-433f-9127-92d0d5b2be1a", "Application.Name", "" },
                    { "f6bf6e19-4a3a-4bd0-8506-da6055cf1774", "Application.Copy", "" },
                    { "5b6153b7-3eb2-4b93-9a13-4331fe6dafff", "BirthdayPath", "http://localhost:50510/api/People/Birthdate?skip=0&take=10" },
                    { "c96e411e-0b3e-4413-bff6-38e09d5d9cba", "Page.PageSize", "3" }
                });
        }
    }
}
