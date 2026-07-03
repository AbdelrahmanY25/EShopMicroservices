using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ordering.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class addnewschemafordatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "oms");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Products",
                newSchema: "oms");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Orders",
                newSchema: "oms");

            migrationBuilder.RenameTable(
                name: "OrderItems",
                newName: "OrderItems",
                newSchema: "oms");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Customers",
                newSchema: "oms");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Products",
                schema: "oms",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "Orders",
                schema: "oms",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "OrderItems",
                schema: "oms",
                newName: "OrderItems");

            migrationBuilder.RenameTable(
                name: "Customers",
                schema: "oms",
                newName: "Customers");
        }
    }
}
