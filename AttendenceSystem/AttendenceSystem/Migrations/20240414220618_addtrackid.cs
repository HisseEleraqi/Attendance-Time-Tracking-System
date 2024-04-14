using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendenceSystem.Migrations
{
    /// <inheritdoc />
    public partial class addtrackid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "Permisions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Permisions",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<int>(
                name: "TrackId",
                table: "Attendences",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attendences_TrackId",
                table: "Attendences",
                column: "TrackId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendences_Tracks_TrackId",
                table: "Attendences",
                column: "TrackId",
                principalTable: "Tracks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendences_Tracks_TrackId",
                table: "Attendences");

            migrationBuilder.DropIndex(
                name: "IX_Attendences_TrackId",
                table: "Attendences");

            migrationBuilder.DropColumn(
                name: "TrackId",
                table: "Attendences");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "Permisions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "Permisions",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
