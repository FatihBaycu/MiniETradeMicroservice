using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniETradeMicroservice.Products.WebAPI.Context;
using MiniETradeMicroservice.Products.WebAPI.Dtos;
using MiniETradeMicroservice.Products.WebAPI.Entities;
using MiniETradeMicroservice.Products.WebAPI.Models;

namespace MiniETradeMicroservice.Products.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            using ApplicationDbContext context = new ApplicationDbContext();
            List<Product> productList = await context.Products.ToListAsync(ct);
            return Ok(new DataResult<List<Product>>
            {
                Data = productList,
                Message = "Products Listed",
                Status = true
            });
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateProductDto createProductDto, CancellationToken ct)
        {
            using ApplicationDbContext context = new ApplicationDbContext();
            if (await context.Products.AnyAsync(p => p.Name == createProductDto.Name, ct))
            {
                return BadRequest(new Result(false, "This product already exists."));
            }

            await context.Products.AddAsync(createProductDto, ct);
            if (await context.SaveChangesAsync(ct) > 0)
            {
                return Ok(new Result(true, "Added successfully."));
            }

            return BadRequest(new Result(false, "Error while adding the product."));
        }
        [HttpGet("seedData")]
        public async Task<IActionResult> SeedData(int? productCount, CancellationToken ct)
        {
            if (productCount == null) { productCount = 100; }
            if (productCount < 1) { productCount = 100; }

            using ApplicationDbContext context = new ApplicationDbContext();

            List<Product> productList = new List<Product>();
            for (int i = 0; i < productCount; i++)
            {
                Faker faker = new Faker();
                productList.Add(new Product
                {
                    Name = faker.Commerce.ProductName(),
                    Price = Convert.ToDecimal(faker.Commerce.Price()),
                    Stock = faker.Commerce.Random.Int(1, 120)
                });
            }
            await context.Products.AddRangeAsync(productList, ct);
            if (await context.SaveChangesAsync(ct) > 0)
            {
                return Ok(new DataResult<List<Product>>
                {
                    Data = productList,
                    Message = productCount + " Product added",
                    Status = true
                });
            }
            return BadRequest(new DataResult<List<Product>>
            {
                Message = "Products not added",
                Status = true
            });
        }
        [HttpPost("change-product-stock")]
        public async Task<IActionResult> ChangeProductStock([FromBody] List<ChangeProductStockDto> changeProductStockDtos, CancellationToken ct)
        {
            int updateCount = 0;
            using ApplicationDbContext context = new ApplicationDbContext();
            foreach (ChangeProductStockDto changeProductStock in changeProductStockDtos)
            {
                Product? product = await context.Products.FirstOrDefaultAsync(p => p.Id == changeProductStock.ProductId, ct);
                if (product == null) { continue; }
                product.Stock -= changeProductStock.Quantity;
                if (await context.SaveChangesAsync(ct) > 0)
                {
                    updateCount++;
                }
            }
            if (updateCount == changeProductStockDtos.Count)
            {
                return Ok(new Result(true, "Updated successfully."));
            }
            if (updateCount == 0 || changeProductStockDtos.Count == 0)
            {
                return Ok(new Result(false, $"Not Updated {changeProductStockDtos.Count}/{updateCount}."));
            }
            return Ok(new Result(true, $"Updated {changeProductStockDtos.Count}/{updateCount} successfully."));
        }
    }
}
