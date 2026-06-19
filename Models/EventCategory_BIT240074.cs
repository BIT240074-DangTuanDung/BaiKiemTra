using System.ComponentModel.DataAnnotations;

namespace BaiKiemTra.Models
{
    public class EventCategory_BIT240074
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name không được để trống")]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public ICollection<Event_BIT240074> Events_BIT240074 { get; set; } = new List<Event_BIT240074>();
    }
}
