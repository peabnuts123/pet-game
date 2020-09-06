using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PetGame.Data.Migrations
{
    public partial class AddTakingTreeDonatedBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DeleteData(
            //     table: "TakingTreeInventoryItems"
            // )

            migrationBuilder.Sql("TRUNCATE \"TakingTreeInventoryItems\" RESTART IDENTITY;");


            migrationBuilder.AddColumn<Guid>(
                name: "DonatedById",
                table: "TakingTreeInventoryItems",
                nullable: false
            );

            migrationBuilder.CreateIndex(
                name: "IX_TakingTreeInventoryItems_DonatedById",
                table: "TakingTreeInventoryItems",
                column: "DonatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_TakingTreeInventoryItems_Users_DonatedById",
                table: "TakingTreeInventoryItems",
                column: "DonatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TakingTreeInventoryItems_Users_DonatedById",
                table: "TakingTreeInventoryItems");

            migrationBuilder.DropIndex(
                name: "IX_TakingTreeInventoryItems_DonatedById",
                table: "TakingTreeInventoryItems");

            migrationBuilder.DropColumn(
                name: "DonatedById",
                table: "TakingTreeInventoryItems");
        }
    }
}
