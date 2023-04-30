using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAn.Migrations
{
    public partial class t : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Product_Cart_ProductId",
            //    table: "Product");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Cart_ProductId",
                table: "Cart");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Product_Cart_ProductId",
            //    table: "Product",
            //    column: "ProductId",
            //    principalTable: "Cart",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Cart_ProductId",
                table: "Product");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Cart_ProductId",
                table: "Cart",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Cart_ProductId",
                table: "Product",
                column: "ProductId",
                principalTable: "Cart",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
