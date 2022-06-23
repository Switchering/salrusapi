
namespace API.JDOs
{
    public class JSONSale
    {
        public string saleID { get; set; }       
        public string number { get; set; }
        public DateTime date { get; set; }
        public DateTime lastChangeDate { get; set; }
        public string supplierArticle { get; set; }
        public string techSize { get; set; }
        public string barcode { get; set; }
        public decimal totalPrice { get; set; }
        public int discountPercent { get; set; }
        public bool isSupply { get; set; }
        public bool isRealization { get; set; }
        public ulong? orderId { get; set; }
        public int promoCodeDiscount { get; set; }
        public string warehouseName { get; set; }
        public string countryName { get; set; }
        public string oblastOkrugName { get; set; }
        public string regionName { get; set; }
        public int incomeID { get; set; }  
        public int spp { get; set; }
        public decimal forPay { get; set; }
        public decimal finishedPrice { get; set; }
        public decimal priceWithDisc { get; set; }
        public ulong nmId { get; set; }
        public string subject { get; set; }
        public string category { get; set; }
        public string brand { get; set; }
        public int IsStorno { get; set; }
        public long odid { get; set; }
    }
}