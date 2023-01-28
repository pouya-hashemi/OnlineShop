using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop.Infrastructure.Persistence.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class m6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "Chk_Price_MinValue",
                table: "Products");

            migrationBuilder.DropCheckConstraint(
                name: "Chk_Quantity_MinValue",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Products",
                newName: "ImagePath");

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    CreatedUserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedUserId = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.CheckConstraint("Chk_Price_MinValue", "Price >= 0");
                    table.CheckConstraint("Chk_Quantity_MinValue", "Quantity >= 1");
                    table.ForeignKey(
                        name: "FK_Carts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddCheckConstraint(
                name: "Chk_Price_MinValue1",
                table: "Products",
                sql: "Price >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "Chk_Quantity_MinValue1",
                table: "Products",
                sql: "Quantity >= 0");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_ProductId",
                table: "Carts",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropCheckConstraint(
                name: "Chk_Price_MinValue1",
                table: "Products");

            migrationBuilder.DropCheckConstraint(
                name: "Chk_Quantity_MinValue1",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Products",
                newName: "ImageUrl");

            migrationBuilder.AddCheckConstraint(
                name: "Chk_Price_MinValue",
                table: "Products",
                sql: "Price >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "Chk_Quantity_MinValue",
                table: "Products",
                sql: "Quantity >= 0");
        }
    }
}
