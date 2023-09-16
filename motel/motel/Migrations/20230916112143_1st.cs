using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace motel.Migrations
{
    /// <inheritdoc />
    public partial class _1st : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rolename = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tiers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tiername = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tiers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tierId = table.Column<int>(type: "int", nullable: false),
                    roleId = table.Column<int>(type: "int", nullable: false),
                    birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    gender = table.Column<bool>(type: "bit", nullable: false),
                    actualFile = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Role_roleId",
                        column: x => x.roleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_Tiers_tierId",
                        column: x => x.tierId,
                        principalTable: "Tiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isHire = table.Column<bool>(type: "bit", nullable: false),
                    datecreatedroom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    area = table.Column<int>(type: "int", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    actualFile = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Post_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Post_Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    categoryId = table.Column<int>(type: "int", nullable: false),
                    postId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post_Category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Post_Category_Category_categoryId",
                        column: x => x.categoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Post_Category_Post_postId",
                        column: x => x.postId,
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Post_Manage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    postId = table.Column<int>(type: "int", nullable: false),
                    userAdminId = table.Column<int>(type: "int", nullable: false),
                    dateapproved = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post_Manage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Post_Manage_Post_postId",
                        column: x => x.postId,
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Post_Manage_User_userAdminId",
                        column: x => x.userAdminId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Post_userId",
                table: "Post",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_Category_categoryId",
                table: "Post_Category",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_Category_postId",
                table: "Post_Category",
                column: "postId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_Manage_postId",
                table: "Post_Manage",
                column: "postId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_Manage_userAdminId",
                table: "Post_Manage",
                column: "userAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_User_roleId",
                table: "User",
                column: "roleId");

            migrationBuilder.CreateIndex(
                name: "IX_User_tierId",
                table: "User",
                column: "tierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Post_Category");

            migrationBuilder.DropTable(
                name: "Post_Manage");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Tiers");
        }
    }
}
