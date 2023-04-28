using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAn.Migrations
{
    public partial class productactiondb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductAction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    NewArrival = table.Column<bool>(type: "bit", nullable: false),
                    BestSeller = table.Column<bool>(type: "bit", nullable: false),
                    Featured = table.Column<bool>(type: "bit", nullable: false),
                    SpecialOffer = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAction", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductAction_ProductId",
                table: "Product",
                column: "ProductId",
                principalTable: "ProductAction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductAction_ProductId",
                table: "Product");

            migrationBuilder.DropTable(
                name: "ProductAction");
        }
    }
}
