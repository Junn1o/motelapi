using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace motel.Migrations
{
    /// <inheritdoc />
    public partial class _2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "Tiers",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "credit",
                table: "Tier_User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "expireDate",
                table: "Tier_User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "regDate",
                table: "Tier_User",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "price",
                table: "Tiers");

            migrationBuilder.DropColumn(
                name: "credit",
                table: "Tier_User");

            migrationBuilder.DropColumn(
                name: "expireDate",
                table: "Tier_User");

            migrationBuilder.DropColumn(
                name: "regDate",
                table: "Tier_User");
        }
    }
}
