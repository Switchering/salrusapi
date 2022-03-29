using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class SalesDto
    {
        [Required]
        public string dateFrom { get; set; }
        public bool Flag { get; set; }
    }
}