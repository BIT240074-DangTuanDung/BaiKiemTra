using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaiKiemTra.Models
{
    public class EventImage_BIT240074
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "ImageUrl không được để trống")]
        public string ImageUrl { get; set; } = string.Empty;

        public bool IsThumbnail { get; set; }

        public int EventId { get; set; }

        [ForeignKey("EventId")]
        public Event_BIT240074? Event_BIT240074 { get; set; }
    }
}
