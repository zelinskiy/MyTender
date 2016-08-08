using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MyTender.Data.Migrations
{
    public partial class Migration8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Prize",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PrizeId = table.Column<int>(nullable: true),
                    RewardedEntityId = table.Column<int>(nullable: false),
                    RewardedEntityType = table.Column<string>(nullable: true),
                    TenderId = table.Column<int>(nullable: true),
                    TenderResponceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prize", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prize_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prize_Prize_PrizeId",
                        column: x => x.PrizeId,
                        principalTable: "Prize",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prize_Tenders_TenderId",
                        column: x => x.TenderId,
                        principalTable: "Tenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prize_TenderResponces_TenderResponceId",
                        column: x => x.TenderResponceId,
                        principalTable: "TenderResponces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddColumn<DateTime>(
                name: "FinishedAt",
                table: "Tenders",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Prize_ApplicationUserId",
                table: "Prize",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Prize_PrizeId",
                table: "Prize",
                column: "PrizeId");

            migrationBuilder.CreateIndex(
                name: "IX_Prize_TenderId",
                table: "Prize",
                column: "TenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Prize_TenderResponceId",
                table: "Prize",
                column: "TenderResponceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishedAt",
                table: "Tenders");

            migrationBuilder.DropTable(
                name: "Prize");
        }
    }
}
