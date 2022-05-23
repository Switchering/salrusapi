using System.ComponentModel.DataAnnotations;

namespace API.Entitites.WBEntities
{
    public class Order
    {
        [Key]
        public string gNumber { get; set; }
        public DateTime date { get; set; }
        public ulong number { get; set; }
    }
}