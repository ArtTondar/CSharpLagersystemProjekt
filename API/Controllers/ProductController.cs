using API.Models;
using API.Repositories;
using API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductRepository _repo;

        public ProductController(IProductRepository repo)
        {
            _repo = repo;
        }

        private IActionResult OkOrNotFound<T>(List<T> list)
        {
            return (list == null || !list.Any()) ? NotFound() : Ok(list);
        }

        [HttpGet("get-product-by-id/{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            try
            {
                Product? product = await _repo.GetProductById(id);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occured while retrieving product.");
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                List<Product> products = await _repo.GetProducts();

                return OkOrNotFound(products);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving products.");
            }
        }

        [HttpGet("get-products-by-name/{name}")]
        public async Task<IActionResult> GetProductsByName(string name)
        {
            try
            {
                List<Product> products = await _repo.GetProductsByName(name);

                return OkOrNotFound(products);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving products.");
            }
        }

        [HttpGet("get-products-by-description/{description}")]
        public async Task<IActionResult> GetProductsByDescription(string description)
        {
            try
            {
                List<Product> products = await _repo.GetProductsByDescription(description);

                return OkOrNotFound(products);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving products.");
            }
        }

        [HttpGet("get-products-by-unit-price/{unitPrice}")]
        public async Task<IActionResult> GetProductsByUnitPrice(decimal unitPrice)
        {
            try
            {
                List<Product> products = await _repo.GetProductsByUnitPrice(unitPrice);

                return OkOrNotFound(products);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving products.");
            }
        }

        [HttpGet("get-products-by-size/{size}")]
        public async Task<IActionResult> GetProductsBySize(int size)
        {
            try
            {
                List<Product> products = await _repo.GetProductsBySize(size);

                return OkOrNotFound(products);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving products.");
            }
        }

        [HttpGet("get-products-by-warehouse/{warehouse}")]
        public async Task<IActionResult> GetProductsByWarehouse(string warehouse)
        {
            try
            {
                List<Product> products = await _repo.GetProductsByWarehouse(warehouse);

                return OkOrNotFound(products);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving products.");
            }
        }

        [HttpGet("get-products-by-unit-stock/{unitStock}")]
        public async Task<IActionResult> GetProductsByUnitStock(int unitStock)
        {
            try
            {
                List<Product> products = await _repo.GetProductsByUnitStock(unitStock);

                return OkOrNotFound(products);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving products.");
            }
        }


        [HttpGet("get-products-by-unit-status/{unitStatus}")]
        public async Task<IActionResult> GetProductsByUnitStatus(UnitStatus unitStatus)
        {
            try
            {
                List<Product> products = await _repo.GetProductsByUnitStatus(unitStatus);

                return OkOrNotFound(products);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving products.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Product createdProduct = await _repo.CreateProduct(product);
                return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occured while creating product.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            var existingProduct = await _repo.GetProductById(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            try
            {
                await _repo.UpdateProduct(product);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating product.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var existingProduct = await _repo.GetProductById(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            try
            {
                await _repo.DeleteProduct(existingProduct);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting product.");
            }
        }
    }
}
