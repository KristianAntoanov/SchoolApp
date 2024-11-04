using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTeacherForeignKeyToRemarksAndGrades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "GuidId",
                keyValue: new Guid("44795218-56d3-4083-a973-126dbd5f90bd"));

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "GuidId",
                keyValue: new Guid("5410f987-f5e2-4c36-812f-f202591beb17"));

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "GuidId",
                keyValue: new Guid("5f82fcaf-733a-44d8-af03-541c970719ab"));

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "GuidId",
                keyValue: new Guid("cbd04e6a-a63d-4f6b-81d1-222f306dda47"));

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "GuidId",
                keyValue: new Guid("d3c9013f-544d-419d-a5fd-8ea3be1845a5"));

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "GuidId",
                keyValue: new Guid("ff202b5a-0e0e-44c7-b035-d179a02d9d0d"));

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherId",
                table: "Remarks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherId",
                table: "Grades",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e77e0d99-7470-4aca-9984-a48e8a9d5152", "AQAAAAIAAYagAAAAEAD4Uh06lvR+r4uuT7+Gbhc+euwoVNiXyLMrkOJwja+whVxhYvoTitdTKNz35FFRKg==", "F150B63E-34F3-4484-A979-2F9B8B15CF8C" });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "GuidId", "ApplicationUserId", "FirstName", "ImageUrl", "JobTitle", "LastName" },
                values: new object[,]
                {
                    { new Guid("3627217b-b181-4122-9097-dc8d0553744e"), new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), "Мария", "/img/No_Image.jpg", "Учител", "Чавдарова" },
                    { new Guid("78f2122e-19aa-48d2-982f-0e5c0faf644d"), new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), "Цветелина", "/img/No_Image.jpg", "Заместник-директор", "Томова" },
                    { new Guid("ee4c776a-60b5-4eee-8c0b-16d40207111c"), new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), "Емилия", "/img/No_Image.jpg", "Заместник-директор", "Истаткова" },
                    { new Guid("f1b8cfda-4f8b-4be5-bd08-acdf95080841"), new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), "Маргарита", "/img/No_Image.jpg", "Учител", "Йорданова" },
                    { new Guid("f44f1451-5a0a-4df5-ab35-9ca4645159e9"), new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), "Стефан", "/img/No_Image.jpg", "Учител", "Николов" },
                    { new Guid("f82854fc-aae5-4be0-8c8b-f7967b2c2058"), new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), "Ани", "/img/No_Image.jpg", "Учител", "Григорова" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Remarks_TeacherId",
                table: "Remarks",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_TeacherId",
                table: "Grades",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Teachers_TeacherId",
                table: "Grades",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "GuidId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Remarks_Teachers_TeacherId",
                table: "Remarks",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "GuidId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Teachers_TeacherId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Remarks_Teachers_TeacherId",
                table: "Remarks");

            migrationBuilder.DropIndex(
                name: "IX_Remarks_TeacherId",
                table: "Remarks");

            migrationBuilder.DropIndex(
                name: "IX_Grades_TeacherId",
                table: "Grades");

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "GuidId",
                keyValue: new Guid("3627217b-b181-4122-9097-dc8d0553744e"));

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "GuidId",
                keyValue: new Guid("78f2122e-19aa-48d2-982f-0e5c0faf644d"));

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "GuidId",
                keyValue: new Guid("ee4c776a-60b5-4eee-8c0b-16d40207111c"));

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "GuidId",
                keyValue: new Guid("f1b8cfda-4f8b-4be5-bd08-acdf95080841"));

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "GuidId",
                keyValue: new Guid("f44f1451-5a0a-4df5-ab35-9ca4645159e9"));

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "GuidId",
                keyValue: new Guid("f82854fc-aae5-4be0-8c8b-f7967b2c2058"));

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Remarks");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Grades");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "464e248f-9049-4db7-ab0d-d29fa437a7d6", "AQAAAAIAAYagAAAAELyBNC4og2ZWJXHRMHPEdAALdt10Rghc7hLSD15BLqiaoFKpfAgiSbKYLem1zzalUg==", "4E84AACC-2DE5-4D08-8761-B93DA7F96F86" });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "GuidId", "ApplicationUserId", "FirstName", "ImageUrl", "JobTitle", "LastName" },
                values: new object[,]
                {
                    { new Guid("44795218-56d3-4083-a973-126dbd5f90bd"), new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), "Цветелина", "/img/No_Image.jpg", "Заместник-директор", "Томова" },
                    { new Guid("5410f987-f5e2-4c36-812f-f202591beb17"), new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), "Маргарита", "/img/No_Image.jpg", "Учител", "Йорданова" },
                    { new Guid("5f82fcaf-733a-44d8-af03-541c970719ab"), new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), "Емилия", "/img/No_Image.jpg", "Заместник-директор", "Истаткова" },
                    { new Guid("cbd04e6a-a63d-4f6b-81d1-222f306dda47"), new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), "Мария", "/img/No_Image.jpg", "Учител", "Чавдарова" },
                    { new Guid("d3c9013f-544d-419d-a5fd-8ea3be1845a5"), new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), "Стефан", "/img/No_Image.jpg", "Учител", "Николов" },
                    { new Guid("ff202b5a-0e0e-44c7-b035-d179a02d9d0d"), new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), "Ани", "/img/No_Image.jpg", "Учител", "Григорова" }
                });
        }
    }
}
