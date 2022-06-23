using System.ComponentModel.DataAnnotations;

namespace API.Entitites.WBEntities
{
    public class Income
    {
        [Key]
        public int? incomeId { get; set; }
        public string number { get; set; }
        public DateTime date { get; set; }        
        public DateTime dateClose { get; set; }
        public string warehouseName { get; set; }
        public List<IncomeDetail> IncomeDetails { get; set; }
    }
}