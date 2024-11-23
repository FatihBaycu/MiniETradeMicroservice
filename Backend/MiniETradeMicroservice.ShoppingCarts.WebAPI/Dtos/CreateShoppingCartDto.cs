using MiniETradeMicroservice.ShoppingCarts.WebAPI.Entities;

namespace MiniETradeMicroservice.ShoppingCarts.WebAPI.Dtos
{
    public class CreateShoppingCartDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        public static implicit operator ShoppingCart(CreateShoppingCartDto createShoppingCartDto)
        {
            return new ShoppingCart
            {
                ProductId = createShoppingCartDto.ProductId,
                Quantity = createShoppingCartDto.Quantity,
            };
        }
    }
}
