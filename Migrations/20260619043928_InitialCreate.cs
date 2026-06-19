using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BaiKiemTra.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventCategories_BIT240074",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCategories_BIT240074", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events_BIT240074",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events_BIT240074", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_BIT240074_EventCategories_BIT240074_EventCategoryId",
                        column: x => x.EventCategoryId,
                        principalTable: "EventCategories_BIT240074",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventImages_BIT240074",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsThumbnail = table.Column<bool>(type: "bit", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventImages_BIT240074", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventImages_BIT240074_Events_BIT240074_EventId",
                        column: x => x.EventId,
                        principalTable: "Events_BIT240074",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "EventCategories_BIT240074",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Các sự kiện hội nghị, hội thảo", "Hội nghị" },
                    { 2, "Các sự kiện thi đấu, cuộc thi", "Hội thi" },
                    { 3, "Các sự kiện tiệc cưới", "Tiệc cưới" }
                });

            migrationBuilder.InsertData(
                table: "Events_BIT240074",
                columns: new[] { "Id", "Description", "EndDate", "EventCategoryId", "Location", "Name", "Price", "StartDate" },
                values: new object[,]
                {
                    { 1, "Hội nghị về phát triển web hiện đại", new DateTime(2026, 7, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "TP. Hồ Chí Minh", "Hội nghị phát triển web 2026", 500000m, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Cuộc thi lập trình dành cho sinh viên", new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Hà Nội", "Cuộc thi lập trình quốc tế", 0m, new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Tiệc cưới lãng mạn", new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Đà Nẵng", "Tiệc cưới John & Mary", 1000000m, new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Hội nghị về trí tuệ nhân tạo", new DateTime(2026, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "TP. Hồ Chí Minh", "Hội thảo AI", 300000m, new DateTime(2026, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Cuộc thi thiết kế đồ họa sáng tạo", new DateTime(2026, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Hà Nội", "Cuộc thi thiết kế đồ họa", 200000m, new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "EventImages_BIT240074",
                columns: new[] { "Id", "EventId", "ImageUrl", "IsThumbnail" },
                values: new object[,]
                {
                    { 1, 1, "https://picsum.photos/400/300?random=1", true },
                    { 2, 1, "https://picsum.photos/400/300?random=2", false },
                    { 3, 2, "https://picsum.photos/400/300?random=3", true },
                    { 4, 3, "https://picsum.photos/400/300?random=4", true },
                    { 5, 4, "https://picsum.photos/400/300?random=5", true },
                    { 6, 5, "https://picsum.photos/400/300?random=6", true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventImages_BIT240074_EventId",
                table: "EventImages_BIT240074",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_BIT240074_EventCategoryId",
                table: "Events_BIT240074",
                column: "EventCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventImages_BIT240074");

            migrationBuilder.DropTable(
                name: "Events_BIT240074");

            migrationBuilder.DropTable(
                name: "EventCategories_BIT240074");
        }
    }
}
