using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace motel.Migrations
{
    /// <inheritdoc />
    public partial class DropTableName2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_User_userId",
                table: "Post");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_User_userId",
                table: "Post",
                column: "userId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.DropTable(name: "Post");
            migrationBuilder.DropTable(name: "Post_Manage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_User_userId",
                table: "Post");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_User_userId",
                table: "Post",
                column: "userId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
