using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dispatcher.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SuperTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuperTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileProcessingTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LinkToFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinesCount = table.Column<int>(type: "int", nullable: false),
                    HighVolumeKeywordsCount = table.Column<long>(type: "bigint", nullable: false),
                    MisspelledKeywordsCount = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SuperTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileProcessingTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileProcessingTasks_SuperTasks_SuperTaskId",
                        column: x => x.SuperTaskId,
                        principalTable: "SuperTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileProcessingTasks_SuperTaskId",
                table: "FileProcessingTasks",
                column: "SuperTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileProcessingTasks");

            migrationBuilder.DropTable(
                name: "SuperTasks");
        }
    }
}
