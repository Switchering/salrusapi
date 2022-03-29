using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class OrdersDto
    {
        [Required]
        public string dateFrom { get; set; }
        public bool Flag { get; set; }
    }
}