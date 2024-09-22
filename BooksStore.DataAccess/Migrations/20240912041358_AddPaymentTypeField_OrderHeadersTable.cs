using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooksStore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentTypeField_OrderHeadersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "OrderHeaders",
                type: "nvarchar(45)",
                maxLength: 45,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "OrderHeaders");
        }
    }
}
