

namespace API.JDOs
{
    public class JSONIncome
    {
        public int incomeId { get; set; }
        public DateTime lastChangeDate { get; set; }
        public string supplierArticle { get; set; }
        public string techSize { get; set; }
        public string barcode { get; set; }
        public int quantity { get; set; }
        public decimal totalPrice { get; set; }
        public ulong nmId { get; set; }
        public string status { get; set; }
        public string number { get; set; }
        public DateTime date { get; set; }        
        public DateTime dateClose { get; set; }
        public string warehouseName { get; set; }  
    }
}