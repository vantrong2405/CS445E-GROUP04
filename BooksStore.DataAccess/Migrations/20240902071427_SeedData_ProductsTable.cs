﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BooksStore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedData_ProductsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ListPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price50 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price100 = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "ISBN", "ListPrice", "Price", "Price100", "Price50", "Title" },
                values: new object[,]
                {
                    { 1, "Billy Spark", "SWD9999001", 99m, 90m, 80m, 85m, "Fortune of Time" },
                    { 2, "Nancy Hoover", "CAW777777701", 40m, 30m, 20m, 25m, "Dark Skies" },
                    { 3, "Julian Button", "RITO5555501", 55m, 50m, 35m, 40m, "Vanish in the Sunset" },
                    { 4, "Abby Muscles", "WS3333333301", 70m, 65m, 55m, 60m, "Cotton Candy" },
                    { 5, "Ron Parker", "SOTJ1111111101", 30m, 27m, 20m, 25m, "Rock in the Ocean" },
                    { 6, "Laura Phantom", "FOT000000001", 25m, 23m, 20m, 22m, "Leaves and Wonders" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
