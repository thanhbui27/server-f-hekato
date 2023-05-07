using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAn.Migrations
{
    public partial class addcommentproduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommentProduct",
                columns: table => new
                {
                    CommentsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rate = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Uid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    createAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentProduct", x => x.CommentsId);
                    table.ForeignKey(
                        name: "FK_CommentProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentProduct_Users_Uid",
                        column: x => x.Uid,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentProduct_ProductId",
                table: "CommentProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentProduct_Uid",
                table: "CommentProduct",
                column: "Uid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentProduct");
        }
    }
}
