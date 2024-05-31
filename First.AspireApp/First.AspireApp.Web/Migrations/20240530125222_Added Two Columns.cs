using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace First.AspireApp.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddedTwoColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AssignedAt",
                table: "SupportTickets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "AssignedToName",
                table: "SupportTickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedAt",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "AssignedToName",
                table: "SupportTickets");
        }
    }
}
