using MiniETradeMicroservice.Orders.WebAPI.Entities;

namespace MiniETradeMicroservice.Orders.WebAPI.DTOs
{
    public class CreateOrderDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public static implicit operator Order(CreateOrderDto createOrderDto)
        {
            return new Order
            {
                Price = createOrderDto.Price,
                Quantity = createOrderDto.Quantity,
                ProductId = createOrderDto.ProductId,
                CreatedDate = DateTime.Now,
            };
        }
    }
}
