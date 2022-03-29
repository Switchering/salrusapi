using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace API.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Incomes",
                columns: table => new
                {
                    incomeId = table.Column<string>(type: "text", nullable: false),
                    number = table.Column<string>(type: "text", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    lastChangeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    supplierArticle = table.Column<string>(type: "text", nullable: false),
                    techSize = table.Column<string>(type: "text", nullable: false),
                    barcode = table.Column<string>(type: "text", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    totalPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    dateClose = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    warehouseName = table.Column<string>(type: "text", nullable: false),
                    nmId = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incomes", x => x.incomeId);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    odid = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    gNumber = table.Column<string>(type: "text", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    lastChangeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    supplierArticle = table.Column<string>(type: "text", nullable: false),
                    techSize = table.Column<string>(type: "text", nullable: false),
                    barcode = table.Column<string>(type: "text", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    totalPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    discountPercent = table.Column<int>(type: "integer", nullable: false),
                    warehouseName = table.Column<string>(type: "text", nullable: false),
                    oblast = table.Column<string>(type: "text", nullable: false),
                    incomeID = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    nmId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    subject = table.Column<string>(type: "text", nullable: false),
                    category = table.Column<string>(type: "text", nullable: false),
                    isCancel = table.Column<bool>(type: "boolean", nullable: false),
                    cancel_dt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    number = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    sticker = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.odid);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "bytea", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    saleID = table.Column<string>(type: "text", nullable: false),
                    gNumber = table.Column<string>(type: "text", nullable: false),
                    number = table.Column<string>(type: "text", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    lastChangeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    supplierArticle = table.Column<string>(type: "text", nullable: false),
                    techSize = table.Column<string>(type: "text", nullable: false),
                    barcode = table.Column<string>(type: "text", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    totalPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    discountPercent = table.Column<int>(type: "integer", nullable: false),
                    isSupply = table.Column<bool>(type: "boolean", nullable: false),
                    isRealization = table.Column<bool>(type: "boolean", nullable: false),
                    orderId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    promoCodeDiscount = table.Column<int>(type: "integer", nullable: false),
                    warehouseName = table.Column<string>(type: "text", nullable: false),
                    countryName = table.Column<string>(type: "text", nullable: false),
                    oblastOkrugName = table.Column<string>(type: "text", nullable: false),
                    regionName = table.Column<string>(type: "text", nullable: false),
                    incomeID = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    odid = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    spp = table.Column<int>(type: "integer", nullable: false),
                    forPay = table.Column<decimal>(type: "numeric", nullable: false),
                    finishedPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    priceWithDisc = table.Column<decimal>(type: "numeric", nullable: false),
                    nmId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    subject = table.Column<string>(type: "text", nullable: false),
                    category = table.Column<string>(type: "text", nullable: false),
                    brand = table.Column<string>(type: "text", nullable: false),
                    IsStorno = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.saleID);
                    table.ForeignKey(
                        name: "FK_Sales_Orders_odid",
                        column: x => x.odid,
                        principalTable: "Orders",
                        principalColumn: "odid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sales_odid",
                table: "Sales",
                column: "odid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Incomes");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
