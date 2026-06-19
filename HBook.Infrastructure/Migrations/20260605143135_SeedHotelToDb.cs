using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HBook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedHotelToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "Id", "CreatedDate", "Description", "ImageUrl", "ModifiedDate", "Name", "Occupancy", "Price", "Sqft" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Comfortable seaside rooms with modern amenities.", "https://images.unsplash.com/photo-1501117716987-c8e79b4e59f3", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SeaView Resort", 2, 199.99000000000001, 420 },
                    { 2, new DateTime(2024, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cozy lodge located in the heart of the mountains.", "https://images.unsplash.com/photo-1542314831-068cd1dbfeeb", new DateTime(2024, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mountain Lodge", 3, 149.5, 360 },
                    { 3, new DateTime(2024, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Modern hotel in the city center close to attractions.", "https://images.unsplash.com/photo-1505691723518-36a3ad12d0a6", new DateTime(2024, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "City Central Hotel", 2, 179.0, 400 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
