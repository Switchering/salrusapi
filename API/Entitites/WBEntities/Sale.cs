using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entitites.WBEntities
{
    public class Sale
    {
        [Key]
        public string saleID { get; set; }       
        public string number { get; set; }
        public DateTime date { get; set; }
        public DateTime lastChangeDate { get; set; }
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
        public int IsStorno { get; set; }

        [ForeignKey("odid")]
        public OrderDetail OrderDetail { get; set; }
        public long odid { get; set; }
    }
}