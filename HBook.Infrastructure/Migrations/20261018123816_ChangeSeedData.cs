using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HBook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Heated outdoor pool overlooking the beach.", "Oceanfront Pool" });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "HotelId", "Name" },
                values: new object[] { "Massage, sauna, and wellness treatments.", 1, "Full-Service Spa" });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "HotelId", "Name" },
                values: new object[] { "Fast wireless internet throughout the property.", 1, "Complimentary Wi-Fi" });

            migrationBuilder.InsertData(
                table: "Amenities",
                columns: new[] { "Id", "Description", "HotelId", "Name" },
                values: new object[,]
                {
                    { 4, "24-hour workspace with printing and meeting support.", 2, "Business Center" },
                    { 5, "Convenient valet service for hotel guests.", 2, "Valet Parking" },
                    { 6, "City views with drinks and light bites.", 2, "Rooftop Lounge" },
                    { 7, "Cozy shared lounge for relaxing after a day outdoors.", 3, "Fireplace Lounge" },
                    { 8, "Secure equipment storage for winter guests.", 3, "Ski Storage" },
                    { 9, "Hot breakfast served daily with local favorites.", 3, "Mountain Breakfast" }
                });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "ImageUrl", "Name", "Price", "Sqft" },
                values: new object[] { "A beachfront resort with ocean-view suites, a full-service spa, and relaxed dining.", "https://images.unsplash.com/photo-1501117716987-c8e79b4e59f3?auto=format&fit=crop&w=1200&q=80", "Seaview Resort & Spa", 259.0, 460 });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "ImageUrl", "Name", "Occupancy", "Price", "Sqft" },
                values: new object[] { "A modern city-center hotel with business amenities, an elegant lobby, and easy access to attractions.", "https://images.unsplash.com/photo-1566073771259-6a8506099945?auto=format&fit=crop&w=1200&q=80", "Grand Meridian Hotel", 2, 189.0, 410 });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "ImageUrl", "Name", "Occupancy", "Price", "Sqft" },
                values: new object[] { "A quiet mountain retreat with warm interiors, scenic views, and cozy guest rooms.", "https://images.unsplash.com/photo-1500048993953-d23a436266cf?auto=format&fit=crop&w=1200&q=80", "Alpine Crest Lodge", 3, 169.0, 380 });

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "Id", "CreatedDate", "Description", "ImageUrl", "ModifiedDate", "Name", "Occupancy", "Price", "Sqft" },
                values: new object[] { 4, new DateTime(2024, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "A tropical stay with spacious suites, poolside lounging, and easy beach access.", "https://images.unsplash.com/photo-1512917774080-9991f1c4c750?auto=format&fit=crop&w=1200&q=80", new DateTime(2024, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Palm Harbor Suites", 4, 219.0, 435 });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 1,
                column: "SpecialDetails",
                value: "Ocean view, king bed, balcony");

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2,
                column: "SpecialDetails",
                value: "Partial sea view, two queen beds");

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 3,
                column: "SpecialDetails",
                value: "City view, king bed, work desk");

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "HotelId", "RoomNumber", "SpecialDetails" },
                values: new object[,]
                {
                    { 4, 2, 202, "City view, two double beds" },
                    { 5, 3, 301, "Mountain view, fireplace, king bed" },
                    { 6, 3, 302, "Forest view, queen bed, reading nook" }
                });

            migrationBuilder.InsertData(
                table: "Amenities",
                columns: new[] { "Id", "Description", "HotelId", "Name" },
                values: new object[,]
                {
                    { 10, "Direct access to the private beach area.", 4, "Beach Access" },
                    { 11, "Scheduled shuttle service for arrivals and departures.", 4, "Airport Shuttle" },
                    { 12, "Separate pool area for family-friendly relaxation.", 4, "Family Pool" }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "HotelId", "RoomNumber", "SpecialDetails" },
                values: new object[,]
                {
                    { 7, 4, 401, "Pool view, king bed, private balcony" },
                    { 8, 4, 402, "Garden view, two queen beds" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name" },
                values: new object[] { "High-speed wireless internet access", "Free Wi-Fi" });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "HotelId", "Name" },
                values: new object[] { "Complimentary continental breakfast", 2, "Breakfast" });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "HotelId", "Name" },
                values: new object[] { "Free on-site parking", 3, "Parking" });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "ImageUrl", "Name", "Price", "Sqft" },
                values: new object[] { "Comfortable seaside rooms with modern amenities.", "https://images.unsplash.com/photo-1501117716987-c8e79b4e59f3", "SeaView Resort", 199.99000000000001, 420 });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "ImageUrl", "Name", "Occupancy", "Price", "Sqft" },
                values: new object[] { "Cozy lodge located in the heart of the mountains.", "https://images.unsplash.com/photo-1542314831-068cd1dbfeeb", "Mountain Lodge", 3, 149.5, 360 });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "ImageUrl", "Name", "Occupancy", "Price", "Sqft" },
                values: new object[] { "Modern hotel in the city center close to attractions.", "https://images.unsplash.com/photo-1505691723518-36a3ad12d0a6", "City Central Hotel", 2, 179.0, 400 });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 1,
                column: "SpecialDetails",
                value: "Sea view, king bed");

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2,
                column: "SpecialDetails",
                value: "Balcony, double bed");

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 3,
                column: "SpecialDetails",
                value: "Mountain view, fireplace");
        }
    }
}
