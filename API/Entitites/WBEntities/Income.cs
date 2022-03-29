using System.ComponentModel.DataAnnotations;

namespace API.Entitites.WBEntities
{
    public class Income
    {
        [Key]
        public string incomeId { get; set; }
        public string number { get; set; }
        public DateTime date { get; set; }
        public DateTime lastChangeDate { get; set; }
        public string supplierArticle { get; set; }
        public string techSize { get; set; }
        public string barcode { get; set; }
        public int quantity { get; set; }
        public decimal totalPrice { get; set; }
        public DateTime dateClose { get; set; }
        public string warehouseName { get; set; }
        public string nmId { get; set; }
        public string status { get; set; }
    }
}