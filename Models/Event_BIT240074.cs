using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaiKiemTra.Models
{
    public class Event_BIT240074
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name không được để trống")]
        public string Name { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Price không được nhỏ hơn 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "StartDate không được để trống")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "EndDate không được để trống")]
        [DateGreaterThan("StartDate", ErrorMessage = "EndDate phải lớn hơn StartDate")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Location không được để trống")]
        public string Location { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "EventCategoryId không được để trống")]
        public int EventCategoryId { get; set; }

        [ForeignKey("EventCategoryId")]
        public EventCategory_BIT240074? EventCategory_BIT240074 { get; set; }

        public ICollection<EventImage_BIT240074> EventImages_BIT240074 { get; set; } = new List<EventImage_BIT240074>();

        public string Status
        {
            get
            {
                var today = DateTime.Now;
                if (today < StartDate)
                    return "Sắp diễn ra";
                if (today >= StartDate && today <= EndDate)
                    return "Đang diễn ra";
                return "Đã kết thúc";
            }
        }
    }

    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateGreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null)
                return new ValidationResult($"Property {_comparisonProperty} not found");

            var comparisonValue = property.GetValue(validationContext.ObjectInstance);
            if (value is DateTime dateValue && comparisonValue is DateTime comparisonDate)
            {
                if (dateValue <= comparisonDate)
                    return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}
