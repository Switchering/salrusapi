using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class IncomesUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "barcode",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "lastChangeDate",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "nmId",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "quantity",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "supplierArticle",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "techSize",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "totalPrice",
                table: "Incomes");

            migrationBuilder.AlterColumn<decimal>(
                name: "incomeId",
                table: "Incomes",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "IncomeDetails",
                columns: table => new
                {
                    id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    incomeId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    lastChangeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    supplierArticle = table.Column<string>(type: "text", nullable: false),
                    techSize = table.Column<string>(type: "text", nullable: false),
                    barcode = table.Column<string>(type: "text", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    totalPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    nmId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeDetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_IncomeDetails_Incomes_incomeId",
                        column: x => x.incomeId,
                        principalTable: "Incomes",
                        principalColumn: "incomeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncomeDetails_incomeId",
                table: "IncomeDetails",
                column: "incomeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncomeDetails");

            migrationBuilder.AlterColumn<string>(
                name: "incomeId",
                table: "Incomes",
                type: "text",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.AddColumn<string>(
                name: "barcode",
                table: "Incomes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "lastChangeDate",
                table: "Incomes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "nmId",
                table: "Incomes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "quantity",
                table: "Incomes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Incomes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "supplierArticle",
                table: "Incomes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "techSize",
                table: "Incomes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "totalPrice",
                table: "Incomes",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
