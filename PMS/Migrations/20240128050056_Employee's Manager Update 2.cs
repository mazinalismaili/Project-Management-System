using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMS.Migrations
{
    /// <inheritdoc />
    public partial class EmployeesManagerUpdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_empManagerId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Manager",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "empManagerId",
                table: "AspNetUsers",
                newName: "ManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_empManagerId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_ManagerId");

            migrationBuilder.CreateTable(
                name: "UserModel",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserModel", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_ManagerId",
                table: "AspNetUsers",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_ManagerId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "UserModel");

            migrationBuilder.RenameColumn(
                name: "ManagerId",
                table: "AspNetUsers",
                newName: "empManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_ManagerId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_empManagerId");

            migrationBuilder.AddColumn<string>(
                name: "Manager",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_empManagerId",
                table: "AspNetUsers",
                column: "empManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
