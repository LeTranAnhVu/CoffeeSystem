using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductService.Dtos;
using ProductService.Models;
using ProductService.Repositories;
using ConflictResult = ProductService.FailResults.ConflictResult;
using NotFoundResult = ProductService.FailResults.NotFoundResult;
using BadRequestResult = ProductService.FailResults.BadRequestResult;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;

        public ProductsController(ILogger<ProductsController> logger, IProductRepository productRepo, IMapper mapper)
        {
            _logger = logger;
            _productRepo = productRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductReadDto>>> Get(
            [FromQuery(Name = "ids")] IReadOnlyList<int> ids,
            [FromQuery(Name = "findByIds")] bool findByIds = false,
            CancellationToken cancellationToken = default)
        {
            if (findByIds)
            {
                var result = await _productRepo.GetProductsByIds(ids, cancellationToken);
                return Ok(result);
            }
            else
            {
                var result = await _productRepo.GetAllAsync(cancellationToken);
                return Ok(result);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductReadDto>> GetProductById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var product = await _productRepo.GetByIdAsync(id, cancellationToken);
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
        public async Task<ActionResult<ProductReadDto>> Create(ProductWriteDto dto, CancellationToken cancellationToken)
        {
            var newProduct = _mapper.Map<Product>(dto);
            var createdProduct = await _productRepo.CreateAsync(newProduct, cancellationToken);
            return Ok(createdProduct);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductReadDto>> Put(int id, ProductWriteDto dto,
            CancellationToken cancellationToken)
        {
            if (id != dto.Id)
            {
                return Conflict(new ConflictResult());
            }

            try
            {
                var product = await _productRepo.GetByIdAsync(id, cancellationToken);
                if (product is null)
                {
                    return NotFound(new NotFoundResult());
                }

                var updatedProduct = _mapper.Map(dto, product);
                await _productRepo.UpdateOneAsync(updatedProduct, cancellationToken);
                return Ok(updatedProduct);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(new BadRequestResult());
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _productRepo.DeleteOneAsync(id, cancellationToken);
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return NotFound(new NotFoundResult());
            }
        }
    }
}