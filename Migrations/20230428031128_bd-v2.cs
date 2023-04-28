using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAn.Migrations
{
    public partial class bdv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductAction_ProductId",
                table: "Product");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAction_ProductId",
                table: "ProductAction",
                column: "ProductId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAction_Product_ProductId",
                table: "ProductAction",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAction_Product_ProductId",
                table: "ProductAction");

            migrationBuilder.DropIndex(
                name: "IX_ProductAction_ProductId",
                table: "ProductAction");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductAction_ProductId",
                table: "Product",
                column: "ProductId",
                principalTable: "ProductAction",
                principalColumn: "Id");
        }
    }
}
