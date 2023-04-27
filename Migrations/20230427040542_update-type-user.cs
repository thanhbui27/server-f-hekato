﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAn.Migrations
{
    public partial class updatetypeuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                table: "Users");
        }
    }
}
