namespace MiniETradeMicroservice.ShoppingCarts.WebAPI.Dtos
{
    public class CreateOrderDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }       
    }
}
