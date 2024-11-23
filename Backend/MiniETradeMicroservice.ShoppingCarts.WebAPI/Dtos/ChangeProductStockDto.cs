namespace MiniETradeMicroservice.ShoppingCarts.WebAPI.Dtos
{
    public class ChangeProductStockDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        public static implicit operator ChangeProductStockDto(CreateOrderDto createOrderDto)
        {
            return new ChangeProductStockDto
            {
                ProductId = createOrderDto.ProductId,
                Quantity = createOrderDto.Quantity
            };
        }
    }
}
