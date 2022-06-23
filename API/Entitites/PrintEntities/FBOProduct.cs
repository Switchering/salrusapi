using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entitites.PrintEntities
{
    public class FBOProduct
    {
        public string Sector { get; set; }
        public string Description { get; set; }
        public string Quant { get; set; }
        public string Box_type { get; set; }
        public string? Request { get; set; }
        public string Barcode1 { get; set; }
        [Key]
        public string BCWB_on_Box { get; set; }
        public string Sticker1 { get; set; }
        public string Sticker2 { get; set; }
        public string Printed { get; set; }
        public string Order_Id { get; set; }
        [ForeignKey("Order_Id")]
        public PrintOrder PrintOrder { get; set; }
        public string? Bar_User { get; set; }
        public string? Printer_Name { get; set; }
        public string? Template_Name { get; set; }
        public DateTime? Print_Time { get; set; }
    }
}