using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElasticSearch.Model;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace ElasticSearch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IElasticClient _elasticClient;

        public ProductsController(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        // GET api/products/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var response = _elasticClient.Get<Product>(id, idx => idx.Index("products"));
            if (!response.Found)
            {
                return NotFound();
            }

            return Ok(response.Source);
        }

        // POST api/products
        [HttpPost]
        public IActionResult Post(Product product)
        {
            var indexResponse = _elasticClient.IndexDocument(product);
            if (indexResponse.IsValid)
            {
                return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
            }
            else
            {
                return BadRequest();
            }
        }

        // PUT api/products/{id}
        [HttpPut("{id}")]
        public IActionResult Put(int id, Product product)
        {
            var response = _elasticClient.Update<Product>(id, idx => idx.Doc(product).Index("products"));
            if (response.IsValid)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        // DELETE api/products/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var response = _elasticClient.Delete<Product>(id, idx => idx.Index("products"));
            if (response.IsValid)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
