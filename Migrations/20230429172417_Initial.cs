using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAn.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[__EFMigrationsHistory]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
