using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dispatcher.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddInvalidLine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFileCorrupted",
                table: "FileProcessingTasks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "InvalidLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LineNumber = table.Column<int>(type: "int", nullable: false),
                    FileProcessingTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvalidLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvalidLines_FileProcessingTasks_FileProcessingTaskId",
                        column: x => x.FileProcessingTaskId,
                        principalTable: "FileProcessingTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvalidLines_FileProcessingTaskId",
                table: "InvalidLines",
                column: "FileProcessingTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_InvalidLines_LineNumber_FileProcessingTaskId",
                table: "InvalidLines",
                columns: new[] { "LineNumber", "FileProcessingTaskId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvalidLines");

            migrationBuilder.DropColumn(
                name: "IsFileCorrupted",
                table: "FileProcessingTasks");
        }
    }
}
