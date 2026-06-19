using Microsoft.EntityFrameworkCore;
using BaiKiemTra.Models;

namespace BaiKiemTra.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<EventCategory_BIT240074> EventCategories_BIT240074 { get; set; }
        public DbSet<Event_BIT240074> Events_BIT240074 { get; set; }
        public DbSet<EventImage_BIT240074> EventImages_BIT240074 { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Set precision for Price to avoid truncation warning
            modelBuilder.Entity<Event_BIT240074>()
                .Property(e => e.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Event_BIT240074>()
                .Property(e => e.Name)
                .HasMaxLength(200);

            modelBuilder.Entity<Event_BIT240074>()
                .Property(e => e.Location)
                .HasMaxLength(200);

            modelBuilder.Entity<Event_BIT240074>()
                .HasIndex(e => new { e.Location, e.StartDate, e.EndDate });

            modelBuilder.Entity<Event_BIT240074>()
                .HasIndex(e => e.EventCategoryId);

            modelBuilder.Entity<EventCategory_BIT240074>().HasData(
                new EventCategory_BIT240074 { Id = 1, Name = "Hội nghị", Description = "Các sự kiện hội nghị, hội thảo" },
                new EventCategory_BIT240074 { Id = 2, Name = "Hội thi", Description = "Các sự kiện thi đấu, cuộc thi" },
                new EventCategory_BIT240074 { Id = 3, Name = "Tiệc cưới", Description = "Các sự kiện tiệc cưới" }
            );

            modelBuilder.Entity<Event_BIT240074>().HasData(
                new Event_BIT240074
                {
                    Id = 1,
                    Name = "Hội nghị phát triển web 2026",
                    Price = 500000,
                    StartDate = new DateTime(2026, 7, 1),
                    EndDate = new DateTime(2026, 7, 3),
                    Location = "TP. Hồ Chí Minh",
                    Description = "Hội nghị về phát triển web hiện đại",
                    EventCategoryId = 1
                },
                new Event_BIT240074
                {
                    Id = 2,
                    Name = "Cuộc thi lập trình quốc tế",
                    Price = 0,
                    StartDate = new DateTime(2026, 8, 15),
                    EndDate = new DateTime(2026, 8, 20),
                    Location = "Hà Nội",
                    Description = "Cuộc thi lập trình dành cho sinh viên",
                    EventCategoryId = 2
                },
                new Event_BIT240074
                {
                    Id = 3,
                    Name = "Tiệc cưới John & Mary",
                    Price = 1000000,
                    StartDate = new DateTime(2026, 6, 25),
                    EndDate = new DateTime(2026, 6, 25),
                    Location = "Đà Nẵng",
                    Description = "Tiệc cưới lãng mạn",
                    EventCategoryId = 3
                },
                new Event_BIT240074
                {
                    Id = 4,
                    Name = "Hội thảo AI",
                    Price = 300000,
                    StartDate = new DateTime(2026, 9, 10),
                    EndDate = new DateTime(2026, 9, 11),
                    Location = "TP. Hồ Chí Minh",
                    Description = "Hội nghị về trí tuệ nhân tạo",
                    EventCategoryId = 1
                },
                new Event_BIT240074
                {
                    Id = 5,
                    Name = "Cuộc thi thiết kế đồ họa",
                    Price = 200000,
                    StartDate = new DateTime(2026, 10, 5),
                    EndDate = new DateTime(2026, 10, 10),
                    Location = "Hà Nội",
                    Description = "Cuộc thi thiết kế đồ họa sáng tạo",
                    EventCategoryId = 2
                }
            );

            modelBuilder.Entity<EventImage_BIT240074>().HasData(
                new EventImage_BIT240074
                {
                    Id = 1,
                    ImageUrl = "https://picsum.photos/400/300?random=1",
                    IsThumbnail = true,
                    EventId = 1
                },
                new EventImage_BIT240074
                {
                    Id = 2,
                    ImageUrl = "https://picsum.photos/400/300?random=2",
                    IsThumbnail = false,
                    EventId = 1
                },
                new EventImage_BIT240074
                {
                    Id = 3,
                    ImageUrl = "https://picsum.photos/400/300?random=3",
                    IsThumbnail = true,
                    EventId = 2
                },
                new EventImage_BIT240074
                {
                    Id = 4,
                    ImageUrl = "https://picsum.photos/400/300?random=4",
                    IsThumbnail = true,
                    EventId = 3
                },
                new EventImage_BIT240074
                {
                    Id = 5,
                    ImageUrl = "https://picsum.photos/400/300?random=5",
                    IsThumbnail = true,
                    EventId = 4
                },
                new EventImage_BIT240074
                {
                    Id = 6,
                    ImageUrl = "https://picsum.photos/400/300?random=6",
                    IsThumbnail = true,
                    EventId = 5
                }
            );
        }
    }
}
