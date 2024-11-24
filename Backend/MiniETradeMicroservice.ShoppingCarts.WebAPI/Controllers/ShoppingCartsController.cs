using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MiniETradeMicroservice.Products.WebAPI.Models;
using MiniETradeMicroservice.ShoppingCarts.WebAPI.Context;
using MiniETradeMicroservice.ShoppingCarts.WebAPI.Dtos;
using MiniETradeMicroservice.ShoppingCarts.WebAPI.Entities;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace MiniETradeMicroservice.ShoppingCarts.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ShoppingCartsController : ControllerBase
    {
        private readonly List<ApiUrl> _apiUrls;
        public ShoppingCartsController(IOptions<List<ApiUrl>> apiUrls)
        {
            _apiUrls = apiUrls.Value;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var productsResult = await getAllProducts();
            if (!productsResult.Status) { return BadRequest(productsResult); }

            using var context = new ApplicationDbContext();
            var shoppingCarts = await context.ShoppingCarts.ToListAsync();

            var shoppingCartDtos = shoppingCarts.Select(p =>
            {
                var product = productsResult?.Data?.FirstOrDefault(q => q.Id == p.ProductId);
                if (product == null) { return null; }
                return new ShoppingCartDto
                {

                    Id = p.Id,
                    Quantity = p.Quantity,
                    ProductId = p.ProductId,
                    ProductName = product.Name,
                    ProductPrice = product.Price
                };
            }).Where(p => p != null).ToList();

            return Ok(new DataResult<List<ShoppingCartDto>>
            {
                Data = shoppingCartDtos,
                Message = "Listed",
                Status = true
            });
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateShoppingCartDto createShoppingCartDto)
        {
            using var context = new ApplicationDbContext();
            await context.ShoppingCarts.AddAsync(createShoppingCartDto);
            return await context.SaveChangesAsync() > 0 ? Ok(new Result(true, "Added")) : BadRequest(new Result(false, "Not added"));
        }
        [HttpGet("create-order")]
        public async Task<IActionResult> CreateOrder()
        {
            var productsResult = await getAllProducts();
            if (!productsResult.Status) { return BadRequest(productsResult); }

            using var context = new ApplicationDbContext();
            var shoppingCarts = await context.ShoppingCarts.ToListAsync();

            var shoppingCartDtos = shoppingCarts.Select(p =>
            {
                var product = productsResult?.Data?.FirstOrDefault(q => q.Id == p.ProductId);
                if (product == null) { return null; }
                return new CreateOrderDto
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity,
                    Price = product.Price
                };
            }).Where(p => p != null).ToList();

            HttpClient client = new HttpClient();
            var orderApi = _apiUrls.FirstOrDefault(x => x.Name == "Orders");
            var orderApiUrl = $"{orderApi?.Host}{orderApi?.Urls?.FirstOrDefault(p => p.EndpointName == "CreateOrder")?.EndpointUrl}";

            string stringJson = JsonSerializer.Serialize(shoppingCartDtos);
            var content = new StringContent(stringJson, Encoding.UTF8, mediaType: "application/json");
            var orderMessage = await client.PostAsync(
                requestUri: orderApiUrl,
                content: content);
            string resultContent = await orderMessage.Content.ReadAsStringAsync();
            Console.WriteLine(orderApiUrl + " - " + resultContent);
            if (orderMessage.IsSuccessStatusCode)
            {
                var productUrlModel = _apiUrls.FirstOrDefault(x => x.Name == "Products");
                var productApiUrl = $"{productUrlModel?.Host}{productUrlModel?.Urls?.FirstOrDefault(p => p.EndpointName == "ChangeProductStock")?.EndpointUrl}";

                HttpClient productChangeStockClient = new HttpClient();
                List<ChangeProductStockDto> changeProductStockDtos = new List<ChangeProductStockDto>(shoppingCartDtos.Select(p => (ChangeProductStockDto)p));

                string changeProductStockStringJson = JsonSerializer.Serialize(changeProductStockDtos);
                var changeProductStockContent = new StringContent(changeProductStockStringJson, Encoding.UTF8, mediaType: "application/json");

                var productStockChangeMessage = await productChangeStockClient.PostAsync(
                    requestUri: productApiUrl,
                    content: changeProductStockContent
                    );
                string productChangeStockClientResultContent = await productStockChangeMessage.Content.ReadAsStringAsync();
                Console.WriteLine(productApiUrl + " - " + productChangeStockClientResultContent);
                context.RemoveRange(shoppingCarts);
                await context.SaveChangesAsync();
            }
            return Ok(new Result(true));
        }
        private async Task<DataResult<List<ProductDto>>> getAllProducts()
        {
            try
            {
                var productApi = _apiUrls.FirstOrDefault(x => x.Name == "Products");
                var productApiUrl = $"{productApi?.Host}{productApi?.Urls?.FirstOrDefault(p => p.EndpointName == "GetAll")?.EndpointUrl}";

                if (string.IsNullOrEmpty(productApiUrl))
                {
                    return new DataResult<List<ProductDto>>("API URL not found.", false, data: null);
                }

                using var context = new ApplicationDbContext();
                var shoppingCarts = await context.ShoppingCarts.ToListAsync();

                HttpClient client = new HttpClient();

                var response = await client.GetAsync(productApiUrl);
                DataResult<List<ProductDto>>? products = new DataResult<List<ProductDto>>();
                if (response.IsSuccessStatusCode)
                {
                    products = await response.Content.ReadFromJsonAsync<DataResult<List<ProductDto>>>();
                }
                return products;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new DataResult<List<ProductDto>>(ex.Message, false, data: null);
            }
        }
    }
}
