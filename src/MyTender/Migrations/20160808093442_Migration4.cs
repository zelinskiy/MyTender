using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MyTender.Migrations
{
    public partial class Migration4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prizes_AspNetUsers_ApplicationUserId",
                table: "Prizes");

            migrationBuilder.DropForeignKey(
                name: "FK_Prizes_Tenders_TenderId",
                table: "Prizes");

            migrationBuilder.DropForeignKey(
                name: "FK_Prizes_TenderResponces_TenderResponceId",
                table: "Prizes");

            migrationBuilder.DropIndex(
                name: "IX_Prizes_ApplicationUserId",
                table: "Prizes");

            migrationBuilder.DropIndex(
                name: "IX_Prizes_TenderId",
                table: "Prizes");

            migrationBuilder.DropIndex(
                name: "IX_Prizes_TenderResponceId",
                table: "Prizes");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Prizes");

            migrationBuilder.DropColumn(
                name: "TenderId",
                table: "Prizes");

            migrationBuilder.DropColumn(
                name: "TenderResponceId",
                table: "Prizes");

            migrationBuilder.CreateTable(
                name: "PrizeEntityRelations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EntityId = table.Column<int>(nullable: false),
                    EntityType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrizeEntityRelations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrizeEntityRelations");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Prizes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenderId",
                table: "Prizes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenderResponceId",
                table: "Prizes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prizes_ApplicationUserId",
                table: "Prizes",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Prizes_TenderId",
                table: "Prizes",
                column: "TenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Prizes_TenderResponceId",
                table: "Prizes",
                column: "TenderResponceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prizes_AspNetUsers_ApplicationUserId",
                table: "Prizes",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Prizes_Tenders_TenderId",
                table: "Prizes",
                column: "TenderId",
                principalTable: "Tenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Prizes_TenderResponces_TenderResponceId",
                table: "Prizes",
                column: "TenderResponceId",
                principalTable: "TenderResponces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
