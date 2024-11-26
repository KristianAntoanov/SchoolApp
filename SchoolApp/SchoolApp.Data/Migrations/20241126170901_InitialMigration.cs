using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    GuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(2083)", maxLength: 2083, nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ApplicationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.GuidId);
                    table.ForeignKey(
                        name: "FK_Teachers_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradeLevel = table.Column<int>(type: "int", nullable: false),
                    SectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classes_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubjectsTeachers",
                columns: table => new
                {
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectsTeachers", x => new { x.TeacherId, x.SubjectId });
                    table.ForeignKey(
                        name: "FK_SubjectsTeachers_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubjectsTeachers_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "GuidId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Absences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    IsExcused = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AddedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Absences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Absences_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Absences_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GradeValue = table.Column<int>(type: "int", nullable: false),
                    AddedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grades_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Grades_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Grades_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "GuidId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Remarks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RemarkText = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    AddedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remarks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Remarks_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Remarks_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Remarks_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "GuidId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubjectsStudents",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectsStudents", x => new { x.StudentId, x.SubjectId });
                    table.ForeignKey(
                        name: "FK_SubjectsStudents_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubjectsStudents_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("167b9fd4-2252-4d5f-9b5d-867599a3e746"), null, "Teacher", "TEACHER" },
                    { new Guid("951a0b30-2bcb-4e61-b0fa-d90512119130"), null, "Parent", "PARENT" },
                    { new Guid("bc1bfaec-7297-48f0-a649-f290de46ad74"), null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("1874d51f-29bc-4669-8f9d-938eaa55e4dd"), 0, "c3a555bc-2699-4e71-8a78-3c21e68b0274", "Tsveti@gmail.com", true, true, null, "TSVETI@GMAIL.COM", "TSVETI@GMAIL.COM", "AQAAAAIAAYagAAAAEMFicfQ7Gc7G2lsTHr6Ml9gL6+6vo8QdQM8DUkl/ax/bqdiRYOHqnCgrv0rKn+eu3Q==", null, false, "405769F8-BE15-475F-A5E5-9025B7FEC684", false, "Tsveti@gmail.com" },
                    { new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), 0, "8a995494-52d7-4932-84ec-525948fbc699", "Stefan@gmail.com", true, true, null, "STEFAN@GMAIL.COM", "STEFAN@GMAIL.COM", "AQAAAAIAAYagAAAAEGLZhQFSjLv5LmkCkWpmAj2C3FGow5ASPlFITyoR8tnmrBa1hOe/UYhYgFKJzmDjRw==", null, false, "2E90C6BF-A6B2-4094-A89B-8719D6F20F36", false, "Stefan@gmail.com" },
                    { new Guid("79eb351b-ed32-4309-9234-88db8555cd3d"), 0, "b11d8751-a7ab-4f81-9032-4644dcce993f", "Margarita@gmail.com", true, true, null, "MARGARITA@GMAIL.COM", "MARGARITA@GMAIL.COM", "AQAAAAIAAYagAAAAEJZrH3WAhWEXCAM42MeyolPdZxFOCZN7sMk27QnetFkPvN90+c2FJZZIoo17Bld4Bg==", null, false, "C6596C09-E5CB-4173-816B-455540DA4943", false, "Margarita@gmail.com" },
                    { new Guid("d040cb3e-ae29-4045-943c-4030a4249476"), 0, "18aa70ed-89d4-49ab-8b68-109d7c7b96ff", "Ani@gmail.com", true, true, null, "ANI@GMAIL.COM", "ANI@GMAIL.COM", "AQAAAAIAAYagAAAAEBz7K66uak/iyQZMFyBPTVLuXZibu5r8n8b04GwGfHnsjKziwiH3eKQhszh39BMOJA==", null, false, "E91A1311-E47B-4CB1-89F9-131E76370BF4", false, "Ani@gmail.com" },
                    { new Guid("e4c5fd5f-c02a-474b-8f51-d4a543f361d3"), 0, "563ff890-a8f0-480b-b007-9666cf171826", "Maria@gmail.com", true, true, null, "MARIA@GMAIL.COM", "MARIA@GMAIL.COM", "AQAAAAIAAYagAAAAEBqz88d+sZIT2UWfxZrUAg6CQ7nvnzwC9dgfDecG3VqRNbaUXrEVn3tzzcs9bv8Dpw==", null, false, "3AD5C3FF-72AA-4FC6-B1A5-7A911F86763C", false, "Maria@gmail.com" },
                    { new Guid("eaad8ef4-d0c4-4cee-bbf0-e1f8e43a6d99"), 0, "0bab1ffc-8656-4aea-b2c3-5ba73a4490c7", "Emilia@gmail.com", true, true, null, "EMILIA@GMAIL.COM", "EMILIA@GMAIL.COM", "AQAAAAIAAYagAAAAEEPZfDaJBD7hpLrgct6X7+xl1avUAaL3CjqGEDxus26wFfP5nY0YBn7ZNJZeXCyVVg==", null, false, "30D4DB53-AF9B-4FCE-B02E-62711A562196", false, "Emilia@gmail.com" }
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
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("bc1bfaec-7297-48f0-a649-f290de46ad74"), new Guid("1874d51f-29bc-4669-8f9d-938eaa55e4dd") });

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
                    { new Guid("0b8ed107-7ddc-46dc-a2df-83a8918f59cc"), new Guid("d040cb3e-ae29-4045-943c-4030a4249476"), "Ани", "/img/No_Image.jpg", "Учител", "Григорова" },
                    { new Guid("2192cdae-685a-4568-a20c-25bfaa7fc15c"), new Guid("79eb351b-ed32-4309-9234-88db8555cd3d"), "Маргарита", "/img/No_Image.jpg", "Учител", "Йорданова" },
                    { new Guid("511df3a5-749c-4d12-a516-6f76c4824072"), new Guid("eaad8ef4-d0c4-4cee-bbf0-e1f8e43a6d99"), "Емилия", "/img/No_Image.jpg", "Заместник-директор", "Истаткова" },
                    { new Guid("b880cdb6-6676-4064-9653-87f8084a905b"), new Guid("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"), "Стефан", "/img/No_Image.jpg", "Учител", "Николов" },
                    { new Guid("ebc52886-531a-4ccc-87ae-cd9004abcdf6"), new Guid("e4c5fd5f-c02a-474b-8f51-d4a543f361d3"), "Мария", "/img/No_Image.jpg", "Учител", "Чавдарова" },
                    { new Guid("f2ffa878-5628-45b4-a8c9-692ff369c35d"), new Guid("1874d51f-29bc-4669-8f9d-938eaa55e4dd"), "Цветелина", "/img/No_Image.jpg", "Заместник-директор", "Томова" }
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
                name: "IX_Absences_StudentId",
                table: "Absences",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Absences_SubjectId",
                table: "Absences",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_SectionId",
                table: "Classes",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_StudentId",
                table: "Grades",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_SubjectId",
                table: "Grades",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_TeacherId",
                table: "Grades",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Remarks_StudentId",
                table: "Remarks",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Remarks_SubjectId",
                table: "Remarks",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Remarks_TeacherId",
                table: "Remarks",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ApplicationUserId",
                table: "Students",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ClassId",
                table: "Students",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectsStudents_SubjectId",
                table: "SubjectsStudents",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectsTeachers_SubjectId",
                table: "SubjectsTeachers",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_ApplicationUserId",
                table: "Teachers",
                column: "ApplicationUserId",
                unique: true,
                filter: "[ApplicationUserId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Absences");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "Remarks");

            migrationBuilder.DropTable(
                name: "SubjectsStudents");

            migrationBuilder.DropTable(
                name: "SubjectsTeachers");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Sections");
        }
    }
}
