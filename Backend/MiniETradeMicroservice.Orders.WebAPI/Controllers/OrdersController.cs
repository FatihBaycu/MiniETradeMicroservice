using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MiniETradeMicroservice.Orders.WebAPI.Context;
using MiniETradeMicroservice.Orders.WebAPI.DTOs;
using MiniETradeMicroservice.Orders.WebAPI.Entities;
using MiniETradeMicroservice.Orders.WebAPI.Models;
using MongoDB.Driver;

namespace MiniETradeMicroservice.Orders.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        IOptions<MongoDBSettings> _mongoDbContext;
        private readonly List<ApiUrl> _apiUrls;

        public OrdersController(IOptions<MongoDBSettings> options, IOptions<List<ApiUrl>> apiUrls)
        {
            _mongoDbContext = options;
            _apiUrls = apiUrls.Value;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            try
            {
                var productApi = _apiUrls.FirstOrDefault(x => x.Name == "Products");
                var productApiUrl = $"{productApi?.Host}{productApi?.Urls?.FirstOrDefault(p => p.EndpointName == "GetAll")?.EndpointUrl}";

                MongoDBContext context = new MongoDBContext(_mongoDbContext);
                var items = context.GetCollection<Order>("Orders");
                var orders = await items.Find<Order>(item => true).ToListAsync(ct);

                DataResult<List<ProductDto>>? products = new DataResult<List<ProductDto>>();

                HttpClient client = new HttpClient();
                var response = await client.GetAsync(productApiUrl);
                if (response.IsSuccessStatusCode)
                {
                    products = await response.Content.ReadFromJsonAsync<DataResult<List<ProductDto>>>(ct);
                }
                List<OrderDto> ordersDto = new List<OrderDto>();

                foreach (var order in orders)
                {
                    ordersDto.Add(new OrderDto
                    {
                        Id = order.Id,
                        CreatedDate = order.CreatedDate,
                        Price = order.Price,
                        ProductId = order.ProductId,
                        Quantity = order.Quantity,
                        ProductName = products?.Data?.FirstOrDefault(p => p?.Id == order.ProductId)?.Name ?? "NULL"
                    });
                }
                return Ok(new DataResult<List<OrderDto>>(true, ordersDto));
            }
            catch (Exception ex)
            {
                return BadRequest(new DataResult<List<OrderDto>>(status: false, data: null, message: ex.Message));
            }
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create(List<CreateOrderDto> createOrderDtos, CancellationToken ct)
        {
            MongoDBContext context = new MongoDBContext(_mongoDbContext);
            var items = context.GetCollection<Order>("Orders");
            try
            {
                var orders = createOrderDtos.Select(p => (Order)p).ToList();
                await items.InsertManyAsync(orders, cancellationToken: ct);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new Result(false, ex.Message));
            }
            return Ok(new Result(true, "Order created successfully"));
        }
    }
}
