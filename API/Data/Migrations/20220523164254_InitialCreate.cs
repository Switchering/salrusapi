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
                    incomeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    number = table.Column<string>(type: "text", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dateClose = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    warehouseName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incomes", x => x.incomeId);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    gNumber = table.Column<string>(type: "text", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    number = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.gNumber);
                });

            migrationBuilder.CreateTable(
                name: "PrintOrders",
                columns: table => new
                {
                    Order_Id = table.Column<string>(type: "text", nullable: false),
                    Order_Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Order_Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrintOrders", x => x.Order_Id);
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
                name: "IncomeDetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    incomeId = table.Column<int>(type: "integer", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    odid = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    lastChangeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    supplierArticle = table.Column<string>(type: "text", nullable: false),
                    techSize = table.Column<string>(type: "text", nullable: false),
                    barcode = table.Column<string>(type: "text", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    totalPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    discountPercent = table.Column<int>(type: "integer", nullable: false),
                    warehouseName = table.Column<string>(type: "text", nullable: false),
                    oblast = table.Column<string>(type: "text", nullable: false),
                    nmId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    subject = table.Column<string>(type: "text", nullable: false),
                    category = table.Column<string>(type: "text", nullable: false),
                    isCancel = table.Column<bool>(type: "boolean", nullable: false),
                    cancel_dt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    number = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    sticker = table.Column<string>(type: "text", nullable: false),
                    gNumber = table.Column<string>(type: "text", nullable: false),
                    incomeId = table.Column<int>(type: "integer", nullable: false),
                    incomeID = table.Column<decimal>(type: "numeric(20,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.odid);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Incomes_incomeId",
                        column: x => x.incomeId,
                        principalTable: "Incomes",
                        principalColumn: "incomeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_gNumber",
                        column: x => x.gNumber,
                        principalTable: "Orders",
                        principalColumn: "gNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FBO",
                columns: table => new
                {
                    BCWB_on_Box = table.Column<string>(type: "text", nullable: false),
                    Sector = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Quant = table.Column<string>(type: "text", nullable: false),
                    Request = table.Column<string>(type: "text", nullable: false),
                    Barcode1 = table.Column<string>(type: "text", nullable: false),
                    Sticker1 = table.Column<string>(type: "text", nullable: false),
                    Sticker2 = table.Column<string>(type: "text", nullable: false),
                    Printed = table.Column<string>(type: "text", nullable: false),
                    Order_Id = table.Column<string>(type: "text", nullable: false),
                    Bar_User = table.Column<string>(type: "text", nullable: false),
                    Printer_Name = table.Column<string>(type: "text", nullable: false),
                    Template_Name = table.Column<string>(type: "text", nullable: false),
                    Print_Time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FBO", x => x.BCWB_on_Box);
                    table.ForeignKey(
                        name: "FK_FBO_PrintOrders_Order_Id",
                        column: x => x.Order_Id,
                        principalTable: "PrintOrders",
                        principalColumn: "Order_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FBS",
                columns: table => new
                {
                    SHKWB = table.Column<string>(type: "text", nullable: false),
                    CodSalrus = table.Column<string>(type: "text", nullable: false),
                    Quant = table.Column<int>(type: "integer", nullable: false),
                    Naim = table.Column<string>(type: "text", nullable: false),
                    Art = table.Column<string>(type: "text", nullable: false),
                    Art_Color = table.Column<string>(type: "text", nullable: false),
                    Sticker1 = table.Column<string>(type: "text", nullable: false),
                    Sticker2 = table.Column<string>(type: "text", nullable: false),
                    Request = table.Column<string>(type: "text", nullable: false),
                    SHK1 = table.Column<string>(type: "text", nullable: false),
                    SHK2 = table.Column<string>(type: "text", nullable: false),
                    SHK3 = table.Column<string>(type: "text", nullable: false),
                    SHK1C = table.Column<string>(type: "text", nullable: false),
                    Printed = table.Column<string>(type: "text", nullable: false),
                    Order_Id = table.Column<string>(type: "text", nullable: false),
                    Bar_User = table.Column<string>(type: "text", nullable: false),
                    Printer_Name = table.Column<string>(type: "text", nullable: false),
                    Template_Name = table.Column<string>(type: "text", nullable: false),
                    Print_Time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FBS", x => x.SHKWB);
                    table.ForeignKey(
                        name: "FK_FBS_PrintOrders_Order_Id",
                        column: x => x.Order_Id,
                        principalTable: "PrintOrders",
                        principalColumn: "Order_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    saleID = table.Column<string>(type: "text", nullable: false),
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
                    spp = table.Column<int>(type: "integer", nullable: false),
                    forPay = table.Column<decimal>(type: "numeric", nullable: false),
                    finishedPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    priceWithDisc = table.Column<decimal>(type: "numeric", nullable: false),
                    nmId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    subject = table.Column<string>(type: "text", nullable: false),
                    category = table.Column<string>(type: "text", nullable: false),
                    brand = table.Column<string>(type: "text", nullable: false),
                    IsStorno = table.Column<int>(type: "integer", nullable: false),
                    odid = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.saleID);
                    table.ForeignKey(
                        name: "FK_Sales_OrderDetails_odid",
                        column: x => x.odid,
                        principalTable: "OrderDetails",
                        principalColumn: "odid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FBO_Order_Id",
                table: "FBO",
                column: "Order_Id");

            migrationBuilder.CreateIndex(
                name: "IX_FBS_Order_Id",
                table: "FBS",
                column: "Order_Id");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeDetails_incomeId",
                table: "IncomeDetails",
                column: "incomeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_gNumber",
                table: "OrderDetails",
                column: "gNumber");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_incomeId",
                table: "OrderDetails",
                column: "incomeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_odid",
                table: "Sales",
                column: "odid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FBO");

            migrationBuilder.DropTable(
                name: "FBS");

            migrationBuilder.DropTable(
                name: "IncomeDetails");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "PrintOrders");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Incomes");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
