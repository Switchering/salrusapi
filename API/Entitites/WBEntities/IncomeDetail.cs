using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entitites.WBEntities
{
    public class IncomeDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int incomeId { get; set; }
        [ForeignKey("incomeId")]
        public Income Income { get; set; }
        public DateTime lastChangeDate { get; set; }
        public string supplierArticle { get; set; }
        public string techSize { get; set; }
        public string barcode { get; set; }
        public int quantity { get; set; }
        public decimal totalPrice { get; set; }
        public ulong nmId { get; set; }
        public string status { get; set; }
    }
}