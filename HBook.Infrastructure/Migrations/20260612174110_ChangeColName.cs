using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HBook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SessionId",
                table: "Bookings",
                newName: "StripeSessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StripeSessionId",
                table: "Bookings",
                newName: "SessionId");
        }
    }
}
