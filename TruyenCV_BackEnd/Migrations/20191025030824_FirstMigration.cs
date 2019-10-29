using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TruyenCV_BackEnd.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Story",
                nullable: true
                );           

            migrationBuilder.CreateTable(
                name: "Bookmark",
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
                    table.PrimaryKey("PK_Bookmark", x => x.Id);
                });                
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {            
            //migrationBuilder.DropTable(
            //    name: "Author");
        }
    }
}
