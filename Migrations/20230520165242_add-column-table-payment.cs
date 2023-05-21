using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAn.Migrations
{
    public partial class addcolumntablepayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "transactionCode",
                table: "payments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "transactionCode",
                table: "payments");
        }
    }
}
