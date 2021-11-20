using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderService.Constants;
using OrderService.Dtos;
using OrderService.Models;
using OrderService.Repositories;
using NotFoundResult = OrderService.FailResults.NotFoundResult;
namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;

        public OrdersController(ILogger<OrdersController> logger, IOrderRepository orderRepository, IMapper mapper)
        {
            _logger = logger;
            _orderRepo = orderRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> Get(CancellationToken cancellationToken)
        {
            var products =  await _orderRepo.GetAllAsync(cancellationToken);
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Order>> GetProductById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var product =  await _orderRepo.GetByIdAsync(id, cancellationToken);
                if (product is null)
                {
                    return NotFound(new NotFoundResult());
                }

                return Ok(product);
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
            var newProduct = _mapper.Map<Order>(dto);

            var createdProduct = await _orderRepo.CreateAsync(newProduct, dto.ProductIds, cancellationToken);
            return Ok(createdProduct);
        }

        [HttpPatch("{id}/updatestatus/{statusCode}")]
        public async Task<ActionResult<Order>> UpdateStatus(int id, OrderStatusCode statusCode, CancellationToken cancellationToken)
        {
            try
            {
                var updatedOrder = await _orderRepo.UpdateStatusOrderAsync(id, statusCode, cancellationToken);
                return Ok(updatedOrder);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return NotFound(new NotFoundResult());
            }
        }

        [HttpPatch("{id}/cancel")]
        public async Task<ActionResult<Order>> CancelOrder(int id, CancellationToken cancellationToken)
        {
            try
            {
                var updatedOrder = await _orderRepo.CancelOrderAsync(id, cancellationToken);
                return Ok(updatedOrder);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return NotFound(new NotFoundResult());
            }
        }
    }
}