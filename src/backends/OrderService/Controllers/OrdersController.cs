using System.Security.Claims;
using AutoMapper;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Dtos;
using OrderService.Models;
using OrderService.Services.OrderProductService;
using NotFoundResult = OrderService.FailResults.NotFoundResult;
using BadRequestResult = OrderService.FailResults.BadRequestResult;
namespace OrderService.Controllers
{
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IOrderProductService _orderService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrdersController(
            ILogger<OrdersController> logger,
            IOrderProductService orderProductService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _logger = logger;
            _orderService = orderProductService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> Get(CancellationToken cancellationToken)
        {
            var orders =  await _orderService.GetOrdersAsync(cancellationToken);
            return Ok(orders);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Order>> GetOrderById(
            int id,  
            [FromQuery(Name = "withPrice")] bool withPrice = false, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var order =  withPrice 
                    ? await _orderService.GetOrderByIdWithPriceAsync(id, cancellationToken) 
                    : await _orderService.GetOrderByIdAsync(id, cancellationToken);
                
                if (order is null)
                {
                    return NotFound(new NotFoundResult());
                }

                return Ok(order);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return NotFound(new NotFoundResult());
            }
        }

        [HttpPost]
        public async Task<ActionResult<Order>> Create(OrderWriteDto dto, CancellationToken cancellationToken)
        {
            try
            {
                // Get user
                var user = _httpContextAccessor.HttpContext?.User;
                var userEmail = user?.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrWhiteSpace(userEmail)) return Unauthorized();

                var newProduct = _mapper.Map<Order>(dto);
                newProduct.OrderedBy = userEmail;
                var createdProduct = await _orderService.CreateOrderAsync(newProduct, dto.ProductIds, cancellationToken);
                return Ok(createdProduct);
            }
            catch (Exception e)
            {
                return BadRequest(new BadRequestResult(e.Message));
            }

        }

        [HttpPatch("{id}/updatestatus/{statusCode}")]
        public async Task<ActionResult<Order>> UpdateStatus(int id, OrderStatusCode statusCode, CancellationToken cancellationToken)
        {
            try
            {
                var updatedOrder = await _orderService.UpdateOrderStatusAsync(id, statusCode, cancellationToken);
                return Ok(updatedOrder);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(new BadRequestResult(e.Message));
            }
        }

        [HttpPatch("{id}/update-to-next-status")]
        public async Task<ActionResult<Order>> UpdateToNextStatus(int id, CancellationToken cancellationToken)
        {
            try
            {
                var updatedOrder = await _orderService.UpdateToNextOrderStatusAsync(id, cancellationToken);
                return Ok(updatedOrder);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(new BadRequestResult(e.Message));
            }
        }

        [HttpPatch("{id}/cancel")]
        public async Task<ActionResult<Order>> CancelOrder(int id, CancellationToken cancellationToken)
        {
            try
            {
                var updatedOrder = await _orderService.CancelOrderAsync(id, cancellationToken);
                return Ok(updatedOrder);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(new BadRequestResult(e.Message));
            }
        }

        [HttpGet("statuses")]
        public ActionResult<List<OrderStatusDto>> GetOrderStatuses()
        {
            var statuses = new List<OrderStatusDto>()
            {
                new (){
                    Name = OrderStatus.Created,
                    Code = OrderStatusCode.Created
                },
                new (){
                    Name = OrderStatus.Paid,
                    Code = OrderStatusCode.Paid
                },
                new (){
                    Name = OrderStatus.Ordered,
                    Code = OrderStatusCode.Ordered
                },
                new (){
                    Name = OrderStatus.Preparing,
                    Code = OrderStatusCode.Preparing
                },
                new (){
                    Name = OrderStatus.Ready,
                    Code = OrderStatusCode.Ready
                },
                new (){
                    Name = OrderStatus.Cancelled,
                    Code = OrderStatusCode.Cancelled
                }
            };

            return statuses;
        }
    }
}