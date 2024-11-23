namespace MiniETradeMicroservice.Products.WebAPI.Dtos
{
    public class ChangeProductStockDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
