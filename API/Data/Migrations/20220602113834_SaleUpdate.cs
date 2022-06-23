using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class SaleUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "barcode",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "brand",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "category",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "discountPercent",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "nmId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "quantity",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "subject",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "supplierArticle",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "techSize",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "totalPrice",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "supplierArticle",
                table: "IncomeDetails");

            migrationBuilder.DropColumn(
                name: "techSize",
                table: "IncomeDetails");

            migrationBuilder.AlterColumn<int>(
                name: "incomeID",
                table: "Sales",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.AddColumn<string>(
                name: "brand",
                table: "OrderDetails",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "brand",
                table: "OrderDetails");

            migrationBuilder.AlterColumn<decimal>(
                name: "incomeID",
                table: "Sales",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "barcode",
                table: "Sales",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "brand",
                table: "Sales",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "category",
                table: "Sales",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "discountPercent",
                table: "Sales",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "nmId",
                table: "Sales",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "quantity",
                table: "Sales",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "subject",
                table: "Sales",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "supplierArticle",
                table: "Sales",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "techSize",
                table: "Sales",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "totalPrice",
                table: "Sales",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "supplierArticle",
                table: "IncomeDetails",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "techSize",
                table: "IncomeDetails",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
