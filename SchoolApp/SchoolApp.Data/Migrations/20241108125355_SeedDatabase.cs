using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("1874d51f-29bc-4669-8f9d-938eaa55e4dd"), 0, "edb481a2-72b6-4f9c-a8cd-46a5d71ec549", "Tsveti@gmail.com", false, true, null, "TSVETI@GMAIL.COM", "TSVETI@GMAIL.COM", "AQAAAAIAAYagAAAAEN4i3ECr+et0gbdUDGzFr3f5SOQ4iSDKboKF5B6HjdHGyLjDnQDMUV9RKNuPmW1DgA==", null, false, "5763609A-7A57-4892-9D1E-BA978BA01434", false, "Tsveti@gmail.com" },
                    { new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), 0, "87eea38d-15e4-4f8b-9f2d-f15475c4515e", "Stefan@gmail.com", false, true, null, "STEFAN@GMAIL.COM", "STEFAN@GMAIL.COM", "AQAAAAIAAYagAAAAEN/fp7r/95WTqlH4MI9c6SRh6HFV8uAZ6+fY+gFCCQwYEzZQ+Z9bH9OxYQ6BiUQQSg==", null, false, "7DAD40F1-6947-43B5-BAF0-F5DE2996CF7F", false, "Stefan@gmail.com" },
                    { new Guid("79eb351b-ed32-4309-9234-88db8555cd3d"), 0, "24d386b5-6e87-4e82-98a4-963140733a61", "Margarita@gmail.com", false, true, null, "MARGARITA@GMAIL.COM", "MARGARITA@GMAIL.COM", "AQAAAAIAAYagAAAAEIj7i7DZOTeE1tsS9gg+NOhINKIos8vTJLYORG5ulw0qQYstbKG7O4G9O+hU+WYGUA==", null, false, "57857659-D227-4FE0-81B8-D7AD5416D926", false, "Margarita@gmail.com" },
                    { new Guid("d040cb3e-ae29-4045-943c-4030a4249476"), 0, "ed67e852-9d9a-4b32-ab20-950fde23e176", "Ani@gmail.com", false, true, null, "ANI@GMAIL.COM", "ANI@GMAIL.COM", "AQAAAAIAAYagAAAAEOZX5oh4Zae8xMIkVmECIRjLwb4nRZrrT/W+WyCEiwwWBPq2+Vuer43fQ5KcZeY8zg==", null, false, "A8509328-1F53-4D67-911F-D5CB43656A67", false, "Ani@gmail.com" },
                    { new Guid("e4c5fd5f-c02a-474b-8f51-d4a543f361d3"), 0, "42ec91fd-fa36-4691-95e2-68d69935975f", "Maria@gmail.com", false, true, null, "MARIA@GMAIL.COM", "MARIA@GMAIL.COM", "AQAAAAIAAYagAAAAENTfCE4LQVkIh2GbS8yRXbgF0iDtBgYgNHBjtIkdzvdwOxj6rf7dPCL7+XZ0aVch/Q==", null, false, "DFB17C9B-C7C2-4C98-98A6-EA6B19D24652", false, "Maria@gmail.com" },
                    { new Guid("eaad8ef4-d0c4-4cee-bbf0-e1f8e43a6d99"), 0, "985edb80-a271-4c54-b1af-1854ac8b6e69", "Emilia@gmail.com", false, true, null, "EMILIA@GMAIL.COM", "EMILIA@GMAIL.COM", "AQAAAAIAAYagAAAAEGGGi44D63wLS5JC8c/juE40GgyRyX/etW7z8cLljeUgmb+BzH490HKPd1xVnrVdBQ==", null, false, "2C8169D2-0188-4847-96BB-4B5222D33B9A", false, "Emilia@gmail.com" }
                });

            migrationBuilder.InsertData(
                table: "Sections",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "А" },
                    { 2, "Б" },
                    { 3, "В" },
                    { 4, "Г" }
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Български език и литература" },
                    { 2, "Математика" },
                    { 3, "Физика" },
                    { 4, "Химия" },
                    { 5, "Програмиране" },
                    { 6, "История" }
                });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "Id", "GradeLevel", "SectionId" },
                values: new object[,]
                {
                    { 1, 5, 1 },
                    { 2, 5, 2 },
                    { 3, 5, 3 },
                    { 4, 6, 1 },
                    { 5, 6, 2 },
                    { 6, 6, 3 }
                });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "GuidId", "ApplicationUserId", "FirstName", "ImageUrl", "JobTitle", "LastName" },
                values: new object[,]
                {
                    { new Guid("4714b09b-5efb-4d05-8130-2bcb7042d416"), new Guid("eaad8ef4-d0c4-4cee-bbf0-e1f8e43a6d99"), "Емилия", "/img/No_Image.jpg", "Заместник-директор", "Истаткова" },
                    { new Guid("7cf90edc-c0d3-41b9-b1e9-c8866d4334cb"), new Guid("d040cb3e-ae29-4045-943c-4030a4249476"), "Ани", "/img/No_Image.jpg", "Учител", "Григорова" },
                    { new Guid("b4a5bf7a-eb65-4307-8c09-66e4a022f433"), new Guid("e4c5fd5f-c02a-474b-8f51-d4a543f361d3"), "Мария", "/img/No_Image.jpg", "Учител", "Чавдарова" },
                    { new Guid("e3e03499-d70c-4c9c-b038-8343f7918cb1"), new Guid("79eb351b-ed32-4309-9234-88db8555cd3d"), "Маргарита", "/img/No_Image.jpg", "Учител", "Йорданова" },
                    { new Guid("ee618fe8-3676-413f-b8fa-0c31bd1b79af"), new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), "Стефан", "/img/No_Image.jpg", "Учител", "Николов" },
                    { new Guid("ef49afad-88c4-4c12-86ae-540c4471d2c0"), new Guid("1874d51f-29bc-4669-8f9d-938eaa55e4dd"), "Цветелина", "/img/No_Image.jpg", "Заместник-директор", "Томова" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "ApplicationUserId", "ClassId", "FirstName", "LastName", "MiddleName" },
                values: new object[,]
                {
                    { 1, new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), 1, "Иван", "Иванов", "Неделинов" },
                    { 2, new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), 1, "Мария", "Петрова", "Викторова" },
                    { 3, new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), 1, "Георги", "Димитров", "Петрунов" },
                    { 4, new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), 2, "Елена", "Станкова", "Георгиева" },
                    { 5, new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), 2, "Петра", "Стайкова", "Петрунова" },
                    { 6, new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), 2, "Георги", "Петров", "Иванов" },
                    { 7, new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), 3, "Мария", "Петрова", "Георгиева" },
                    { 8, new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), 3, "Иван", "Иванов", "Стефанов" },
                    { 9, new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), 3, "Елена", "Василева", "Николова" },
                    { 10, new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), 4, "Никола", "Димитров", "Петров" },
                    { 11, new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), 4, "Петър", "Димитров", "Георгиев" },
                    { 12, new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), 4, "Даниела", "Маринова", "Иванова" },
                    { 13, new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), 5, "Александър", "Стоянов", "Николов" },
                    { 14, new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), 5, "Калина", "Димитрова", "Петкова" },
                    { 15, new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), 5, "Радослав", "Петров", "Георгиев" },
                    { 16, new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), 6, "Борис", "Караджов", "Стефанов" },
                    { 17, new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), 6, "Антония", "Тодорова", "Илиева" },
                    { 18, new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), 6, "Виктор", "Колев", "Алексиев" }
                });

            migrationBuilder.InsertData(
                table: "SubjectsStudents",
                columns: new[] { "StudentId", "SubjectId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 1, 4 },
                    { 1, 5 },
                    { 1, 6 },
                    { 2, 1 },
                    { 2, 2 },
                    { 2, 3 },
                    { 2, 4 },
                    { 2, 5 },
                    { 2, 6 },
                    { 3, 1 },
                    { 3, 2 },
                    { 3, 3 },
                    { 3, 4 },
                    { 3, 5 },
                    { 3, 6 },
                    { 4, 1 },
                    { 4, 2 },
                    { 4, 3 },
                    { 4, 4 },
                    { 4, 5 },
                    { 4, 6 },
                    { 5, 1 },
                    { 5, 2 },
                    { 5, 3 },
                    { 5, 4 },
                    { 5, 5 },
                    { 5, 6 },
                    { 6, 1 },
                    { 6, 2 },
                    { 6, 3 },
                    { 6, 4 },
                    { 6, 5 },
                    { 6, 6 },
                    { 7, 1 },
                    { 7, 2 },
                    { 7, 3 },
                    { 7, 4 },
                    { 7, 5 },
                    { 7, 6 },
                    { 8, 1 },
                    { 8, 2 },
                    { 8, 3 },
                    { 8, 4 },
                    { 8, 5 },
                    { 8, 6 },
                    { 9, 1 },
                    { 9, 2 },
                    { 9, 3 },
                    { 9, 4 },
                    { 9, 5 },
                    { 9, 6 },
                    { 10, 1 },
                    { 10, 2 },
                    { 10, 3 },
                    { 10, 4 },
                    { 10, 5 },
                    { 10, 6 },
                    { 11, 1 },
                    { 11, 2 },
                    { 11, 3 },
                    { 11, 4 },
                    { 11, 5 },
                    { 11, 6 },
                    { 12, 1 },
                    { 12, 2 },
                    { 12, 3 },
                    { 12, 4 },
                    { 12, 5 },
                    { 12, 6 },
                    { 13, 1 },
                    { 13, 2 },
                    { 13, 3 },
                    { 13, 4 },
                    { 13, 5 },
                    { 13, 6 },
                    { 14, 1 },
                    { 14, 2 },
                    { 14, 3 },
                    { 14, 4 },
                    { 14, 5 },
                    { 14, 6 },
                    { 15, 1 },
                    { 15, 2 },
                    { 15, 3 },
                    { 15, 4 },
                    { 15, 5 },
                    { 15, 6 },
                    { 16, 1 },
                    { 16, 2 },
                    { 16, 3 },
                    { 16, 4 },
                    { 16, 5 },
                    { 16, 6 },
                    { 17, 1 },
                    { 17, 2 },
                    { 17, 3 },
                    { 17, 4 },
                    { 17, 5 },
                    { 17, 6 },
                    { 18, 1 },
                    { 18, 2 },
                    { 18, 3 },
                    { 18, 4 },
                    { 18, 5 },
                    { 18, 6 }
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
                table: "Sections",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 1, 5 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 1, 6 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 2, 4 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 2, 5 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 2, 6 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 3, 2 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 3, 4 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 3, 5 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 3, 6 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 4, 2 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 4, 3 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 4, 4 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 4, 5 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 4, 6 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 5, 2 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 5, 3 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 5, 4 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 5, 5 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 5, 6 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 6, 1 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 6, 2 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 6, 3 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 6, 4 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 6, 5 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 6, 6 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 7, 1 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 7, 2 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 7, 3 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 7, 4 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 7, 5 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 7, 6 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 8, 1 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 8, 2 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 8, 3 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 8, 4 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 8, 5 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 8, 6 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 9, 1 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 9, 2 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 9, 3 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 9, 4 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 9, 5 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 9, 6 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 10, 1 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 10, 2 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 10, 3 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 10, 4 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 10, 5 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 10, 6 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 11, 1 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 11, 2 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 11, 3 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 11, 4 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 11, 5 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 11, 6 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 12, 1 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 12, 2 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 12, 3 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 12, 4 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 12, 5 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 12, 6 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 13, 1 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 13, 2 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 13, 3 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 13, 4 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 13, 5 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 13, 6 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 14, 1 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 14, 2 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 14, 3 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 14, 4 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 14, 5 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 14, 6 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 15, 1 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 15, 2 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 15, 3 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 15, 4 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 15, 5 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 15, 6 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 16, 1 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 16, 2 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 16, 3 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 16, 4 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 16, 5 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 16, 6 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 17, 1 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 17, 2 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 17, 3 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 17, 4 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 17, 5 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 17, 6 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 18, 1 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 18, 2 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 18, 3 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 18, 4 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 18, 5 });

            migrationBuilder.DeleteData(
                table: "SubjectsStudents",
                keyColumns: new[] { "StudentId", "SubjectId" },
                keyValues: new object[] { 18, 6 });

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "GuidId",
                keyValue: new Guid("4714b09b-5efb-4d05-8130-2bcb7042d416"));

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "GuidId",
                keyValue: new Guid("7cf90edc-c0d3-41b9-b1e9-c8866d4334cb"));

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "GuidId",
                keyValue: new Guid("b4a5bf7a-eb65-4307-8c09-66e4a022f433"));

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "GuidId",
                keyValue: new Guid("e3e03499-d70c-4c9c-b038-8343f7918cb1"));

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "GuidId",
                keyValue: new Guid("ee618fe8-3676-413f-b8fa-0c31bd1b79af"));

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "GuidId",
                keyValue: new Guid("ef49afad-88c4-4c12-86ae-540c4471d2c0"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1874d51f-29bc-4669-8f9d-938eaa55e4dd"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("79eb351b-ed32-4309-9234-88db8555cd3d"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d040cb3e-ae29-4045-943c-4030a4249476"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e4c5fd5f-c02a-474b-8f51-d4a543f361d3"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("eaad8ef4-d0c4-4cee-bbf0-e1f8e43a6d99"));

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"));

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Remarks");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Grades");
        }
    }
}
