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
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), 0, "464e248f-9049-4db7-ab0d-d29fa437a7d6", "Peshko@abv.com", false, true, null, "PESHKO@ABV.COM", "PESHKO@ABV.COM", "AQAAAAIAAYagAAAAELyBNC4og2ZWJXHRMHPEdAALdt10Rghc7hLSD15BLqiaoFKpfAgiSbKYLem1zzalUg==", null, false, "4E84AACC-2DE5-4D08-8761-B93DA7F96F86", false, "Peshko@abv.com" });

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
                    { new Guid("44795218-56d3-4083-a973-126dbd5f90bd"), new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), "Цветелина", "/img/No_Image.jpg", "Заместник-директор", "Томова" },
                    { new Guid("5410f987-f5e2-4c36-812f-f202591beb17"), new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), "Маргарита", "/img/No_Image.jpg", "Учител", "Йорданова" },
                    { new Guid("5f82fcaf-733a-44d8-af03-541c970719ab"), new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), "Емилия", "/img/No_Image.jpg", "Заместник-директор", "Истаткова" },
                    { new Guid("cbd04e6a-a63d-4f6b-81d1-222f306dda47"), new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), "Мария", "/img/No_Image.jpg", "Учител", "Чавдарова" },
                    { new Guid("d3c9013f-544d-419d-a5fd-8ea3be1845a5"), new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), "Стефан", "/img/No_Image.jpg", "Учител", "Николов" },
                    { new Guid("ff202b5a-0e0e-44c7-b035-d179a02d9d0d"), new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), "Ани", "/img/No_Image.jpg", "Учител", "Григорова" }
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
