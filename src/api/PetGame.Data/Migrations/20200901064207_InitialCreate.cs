using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PetGame.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AuthId = table.Column<string>(nullable: false),
                    Username = table.Column<string>(maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TakingTreeInventoryItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ItemId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TakingTreeInventoryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TakingTreeInventoryItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerInventoryItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    ItemId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerInventoryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerInventoryItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerInventoryItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("0e21e615-b04e-4213-9d4d-e35e981b9194"), "Crud" },
                    { new Guid("0b5aab7c-3420-4672-9a51-d8f9f8a7b97e"), "Coiled Rope" },
                    { new Guid("fea41875-c84c-4f60-b490-8b4cd2f648e0"), "Ripe Orange" },
                    { new Guid("dab554ee-50e6-417d-b760-2eca12bcae79"), "Tacking Nails" },
                    { new Guid("beccb327-2703-4b19-8b67-bb77b63a7c0c"), "Elastic Band" },
                    { new Guid("55959d76-a087-4c60-aea7-619f7c077c6f"), "Elegant Flask" },
                    { new Guid("f0147ee7-6fd2-409b-97bb-4796ca3ea099"), "Gold Ring" },
                    { new Guid("2ef5a467-84f1-4dbb-b353-7c14c67f0c80"), "WORLDS BEST DAD Mug" },
                    { new Guid("0504bcea-ad14-4bd4-9263-309fadda52a6"), "Left-hand Glove" },
                    { new Guid("9a270da3-bcdd-46f5-9c29-af29b34cfac2"), "Right-hand Glove" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerInventoryItems_ItemId",
                table: "PlayerInventoryItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerInventoryItems_UserId",
                table: "PlayerInventoryItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TakingTreeInventoryItems_ItemId",
                table: "TakingTreeInventoryItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AuthId",
                table: "Users",
                column: "AuthId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerInventoryItems");

            migrationBuilder.DropTable(
                name: "TakingTreeInventoryItems");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Items");
        }
    }
}
