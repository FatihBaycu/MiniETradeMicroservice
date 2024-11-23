using MiniETradeMicroservice.Products.WebAPI.Entities;

namespace MiniETradeMicroservice.Products.WebAPI.Dtos
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public static implicit operator Product(CreateProductDto dto)
        {
            return new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Stock = dto.Stock
            };
        }
    }
}
