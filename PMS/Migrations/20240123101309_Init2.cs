using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMS.Migrations
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_Status_StatusId",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_Project_ProjectId",
                table: "Task");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_Status_StatusId",
                table: "Task");

            migrationBuilder.AddColumn<string>(
                name: "StatusId1",
                table: "Project",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_StatusId1",
                table: "Project",
                column: "StatusId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Status_StatusId",
                table: "Project",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Status_StatusId1",
                table: "Project",
                column: "StatusId1",
                principalTable: "Status",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Project_ProjectId",
                table: "Task",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Status_StatusId",
                table: "Task",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_Status_StatusId",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Status_StatusId1",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_Project_ProjectId",
                table: "Task");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_Status_StatusId",
                table: "Task");

            migrationBuilder.DropIndex(
                name: "IX_Project_StatusId1",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "StatusId1",
                table: "Project");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Status_StatusId",
                table: "Project",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Project_ProjectId",
                table: "Task",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Status_StatusId",
                table: "Task",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "Id");
        }
    }
}
