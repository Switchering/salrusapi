using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entitites.WBEntities
{
    public class Product
    {
        public string supplierArticle { get; set; }
        public string techSize { get; set; }
        public string barcode { get; set; }
        public decimal forPay { get; set; }
        public decimal finishedPrice { get; set; }
        public decimal priceWithDisc { get; set; }
        [Key]
        public ulong nmId { get; set; }
        public string subject { get; set; }
        public string category { get; set; }
        public string brand { get; set; }
    }
}