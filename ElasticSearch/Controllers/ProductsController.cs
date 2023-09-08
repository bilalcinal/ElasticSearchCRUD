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
    [Route("api/[controller]/[action]")]
    public class ProductsController : ControllerBase
    {
        private readonly IElasticClient _elasticClient;

        public ProductsController(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        [HttpGet]
        public IActionResult Get(int id)
        {
            var response = _elasticClient.Get<Product>(id, idx => idx.Index("products"));
            if (!response.Found)
            {
                return NotFound();
            }

            return Ok(response.Source);
        }

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

        [HttpPut]
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

        [HttpDelete]
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
