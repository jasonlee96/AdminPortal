using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class JwtToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginDate",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastLoginIP",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RoleGroup",
                table: "Role",
                type: "nvarchar(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Merchant",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Merchant",
                type: "nvarchar(45)",
                maxLength: 45,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Lat",
                table: "Merchant",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Lng",
                table: "Merchant",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Merchant",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Merchant",
                type: "nvarchar(45)",
                maxLength: 45,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastLoginDate",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LastLoginIP",
                table: "User");

            migrationBuilder.DropColumn(
                name: "RoleGroup",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Merchant");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Merchant");

            migrationBuilder.DropColumn(
                name: "Lat",
                table: "Merchant");

            migrationBuilder.DropColumn(
                name: "Lng",
                table: "Merchant");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Merchant");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Merchant");
        }
    }
}
