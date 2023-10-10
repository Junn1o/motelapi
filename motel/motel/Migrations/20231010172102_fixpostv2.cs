using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace motel.Migrations
{
    /// <inheritdoc />
    public partial class fixpostv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Post_Manage_Id",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "Idpm",
                table: "Post_Category",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Post",
                newName: "Ids");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Post_Manage_Ids",
                table: "Post",
                column: "Ids",
                principalTable: "Post_Manage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Post_Manage_Ids",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Post_Category",
                newName: "Idpm");

            migrationBuilder.RenameColumn(
                name: "Ids",
                table: "Post",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Post_Manage_Id",
                table: "Post",
                column: "Id",
                principalTable: "Post_Manage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
