using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TruyenCV_BackEnd.Migrations
{
    public partial class AddSubcription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reading",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    StatusId = table.Column<bool>(nullable: false),
                    StoryId = table.Column<Guid>(nullable: false),
                    ChapterId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reading", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    StatusId = table.Column<bool>(nullable: false),
                    StoryId = table.Column<Guid>(nullable: false),
                    ChapterId = table.Column<Guid>(nullable: false),
                    IsNewest = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reading");

            migrationBuilder.DropTable(
                name: "Subscription");
        }
    }
}
