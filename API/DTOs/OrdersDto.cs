using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class OrdersDto
    {
        [Required]
        public string Date_start { get; set; }
        public int? Status { get; set; }
        [Required]
        public int Take  { get; set; }
        [Required]
        public int Skip  { get; set; }
        public int? Id { get; set; }
        public string? Date_end { get; set; }
    }
}