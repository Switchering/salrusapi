namespace API.Entitites
{
    public class Barcode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public Product ProductId { get; set; }
    }
}