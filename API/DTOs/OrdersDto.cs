using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class OrdersDto
    {
        // [Required]
        public string Date_start { get; set; }
        // public string date_end { get; set; }
        // public int status { get; set; }
        // [Required]
        public int Take  { get; set; }
        // [Required]
        public int Skip  { get; set; }
        // public int id { get; set; }
    }
}