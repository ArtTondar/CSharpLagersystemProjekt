using API.Models;
using API.Repositories.DbAccess;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductAccess _productAccess;

        public ProductController(ProductAccess productAccess) 
        {
            _productAccess = productAccess;
        }

        [HttpGet("get-product-by-id/{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            (Product product, Exception e) = await _productAccess.GetProductById(id);
            if (product != null)
            {
                return Ok(product);
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _productAccess.GetProducts();

                if (products == null || !products.Any())
                {
                    return NotFound();
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving products.");
            }
        }

        [HttpGet("get-products-by-name/{name}")]
        public async Task<IActionResult> GetProductsByName(string name)
        {
            try
            {
                var products = await _productAccess.GetProductsByName(name);

                if (products == null || !products.Any())
                {
                    return NotFound();
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving products.");
            }
        }

        [HttpGet("get-products-by-description/{description}")]
        public async Task<IActionResult> GetProductsByDescription(string description)
        {
            try
            {
                var products = await _productAccess.GetProductsByDescription(description);

                if (products == null || !products.Any())
                {
                    return NotFound();
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving products.");
            }
        }

        [HttpGet("get-products-by-unit-price/{unitPrice}")]
        public async Task<IActionResult> GetProductsByUnitPrice(decimal unitPrice)
        {
            try
            {
                var products = await _productAccess.GetProductsByUnitPrice(unitPrice);

                if (products == null || !products.Any())
                {
                    return NotFound();
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving products.");
            }
        }

        [HttpGet("get-products-by-size/{size}")]
        public async Task<IActionResult> GetProductsBySize(int size)
        {
            try
            {
                var products = await _productAccess.GetProductsBySize(size);

                if (products == null || !products.Any())
                {
                    return NotFound();
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving products.");
            }
        }

        [HttpGet("get-products-by-warehouse/{warehouse}")]
        public async Task<IActionResult> GetProductsByWarehouse(string warehouse)
        {
            try
            {
                var products = await _productAccess.GetProductsByWarehouse(warehouse);

                if (products == null || !products.Any())
                {
                    return NotFound();
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving products.");
            }
        }

        [HttpGet("get-products-by-unit-stock/{unitStock}")]
        public async Task<IActionResult> GetProductsByUnitStock(int unitStock)
        {
            try
            {
                var products = await _productAccess.GetProductsByUnitStock(unitStock);

                if (products == null || !products.Any())
                {
                    return NotFound();
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving products.");
            }
        }


        [HttpGet("get-products-by-unit-status/{unitStatus}")]
        public async Task<IActionResult> GetProductsByUnitStatus(UnitStatus unitStatus)
        {
            try
            {
                var products = await _productAccess.GetProductsByUnitStatus(unitStatus);

                if (products == null || !products.Any())
                {
                    return NotFound();
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving products.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdProduct = await _productAccess.CreateProduct(product);
                return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(Guid id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest("ID mismatch");
            }

            try
            {
                var updated = await _productAccess.UpdateProduct(product);
                if (!updated)
                {
                    return NotFound($"Product with ID {id} not found");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                var deleted = await _productAccess.DeleteProduct(id);
                if (!deleted)
                {
                    return NotFound($"Product with ID {id} not found");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
