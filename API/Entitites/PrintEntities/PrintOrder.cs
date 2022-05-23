using System.ComponentModel.DataAnnotations;

namespace API.Entitites.WBEntities
{
    public class PrintOrder
    {
        [Key]
        public string Order_Id { get; set; }
        public DateTime Order_Date { get; set; }
        public string Order_Type { get; set; }
    }
}