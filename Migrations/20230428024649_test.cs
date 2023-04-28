using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAn.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductAction_ProductId",
                table: "Product");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductAction_ProductId",
                table: "Product",
                column: "ProductId",
                principalTable: "ProductAction",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductAction_ProductId",
                table: "Product");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductAction_ProductId",
                table: "Product",
                column: "ProductId",
                principalTable: "ProductAction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
