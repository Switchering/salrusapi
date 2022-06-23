using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entitites.PrintEntities
{
    public class FBSProduct
    {
        public string CodSalrus { get; set; }
        public int Quant { get; set; }
        public string? Naim { get; set; }
        public string Art { get; set; }
        public string Art_Color { get; set; }
        [Key]
        public string SHKWB { get; set; }
        public string Sticker1 { get; set; }
        public string Sticker2 { get; set; }
        public string Request { get; set; }
        public string? SHK1 { get; set; }  = null!;
        public string? SHK2 { get; set; }  = null!;
        public string? SHK3 { get; set; }  = null!;
        public string? SHK1C { get; set; }  = null!;
        public string? Printed { get; set; }  = null!;
        public string Order_Id { get; set; }
        [ForeignKey("Order_Id")]
        public PrintOrder PrintOrder { get; set; }  = null!;
        public string? Bar_User { get; set; }  = null!;
        public string? Printer_Name { get; set; }  = null!;
        public string? Template_Name { get; set; }  = null!;
        public DateTime Print_Time { get; set; }
    }
}