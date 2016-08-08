using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyTender.Migrations
{
    public partial class Migration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Prizes",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Prizes",
                nullable: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCreatedByUser",
                table: "Prizes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                table: "Prizes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCreatedByUser",
                table: "Prizes");

            migrationBuilder.DropColumn(
                name: "PictureUrl",
                table: "Prizes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Prizes",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Prizes",
                nullable: true);
        }
    }
}
