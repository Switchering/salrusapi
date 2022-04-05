using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class IncomesDto
    {
        [Required]
        public string dateFrom { get; set; }
    }
}