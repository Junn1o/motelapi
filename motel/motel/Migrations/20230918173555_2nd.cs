using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace motel.Migrations
{
    /// <inheritdoc />
    public partial class _2nd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Manage_User_userAdminId",
                table: "Post_Manage");

            migrationBuilder.AlterColumn<int>(
                name: "userAdminId",
                table: "Post_Manage",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "dateapproved",
                table: "Post_Manage",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Manage_User_userAdminId",
                table: "Post_Manage",
                column: "userAdminId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Manage_User_userAdminId",
                table: "Post_Manage");

            migrationBuilder.AlterColumn<int>(
                name: "userAdminId",
                table: "Post_Manage",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "dateapproved",
                table: "Post_Manage",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Manage_User_userAdminId",
                table: "Post_Manage",
                column: "userAdminId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
