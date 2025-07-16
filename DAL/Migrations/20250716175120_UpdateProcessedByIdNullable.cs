using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProcessedByIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BorrowRequests",
                keyColumn: "RequestId",
                keyValue: 1,
                columns: new[] { "BorrowDate", "RequestDate", "ReturnDate" },
                values: new object[] { new DateTime(2025, 7, 18, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 7, 17, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 7, 24, 0, 0, 0, 0, DateTimeKind.Local) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BorrowRequests",
                keyColumn: "RequestId",
                keyValue: 1,
                columns: new[] { "BorrowDate", "RequestDate", "ReturnDate" },
                values: new object[] { new DateTime(2025, 7, 9, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 7, 8, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 7, 15, 0, 0, 0, 0, DateTimeKind.Local) });
        }
    }
}
