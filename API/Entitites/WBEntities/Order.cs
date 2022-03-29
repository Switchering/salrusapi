using System.ComponentModel.DataAnnotations;

namespace API.Entitites.WBEntities
{
    public class Order
    {
        
        public string gNumber { get; set; }
        public DateTime date { get; set; }
        public DateTime lastChangeDate { get; set; }
        public string supplierArticle { get; set; }
        public string techSize { get; set; }
        public string barcode { get; set; }
        public int quantity { get; set; }
        public decimal totalPrice { get; set; }
        public int discountPercent { get; set; }
        public string warehouseName { get; set; }
        public string oblast { get; set; }
        public ulong? incomeID { get; set; }
        [Key]
        public ulong odid { get; set; }
        public ulong nmId { get; set; }
        public string subject { get; set; }
        public string category { get; set; }
        public bool isCancel { get; set; }
        public DateTime cancel_dt { get; set; }
        public ulong number { get; set; }
        public string sticker { get; set; }
        public List<Sale> Sales { get; set; }
    }
}