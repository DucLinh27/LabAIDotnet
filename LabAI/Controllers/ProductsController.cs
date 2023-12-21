// Controllers/ProductsController.cs
using Microsoft.AspNetCore.Mvc;
using LabAI.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using LabAI.Data;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly MongoDBContext _mongoDBContext;

    public ProductsController(MongoDBContext mongoDBContext)
    {
        _mongoDBContext = mongoDBContext;
    }

    // GET: api/products
    [HttpGet]
    public IActionResult GetProducts()
    {
        var products = _mongoDBContext.Products.Find(_ => true).ToList();
        return Ok(products);
    }

    // GET: api/products/5
    [HttpGet("{id}")]
    public IActionResult GetProduct(string id)
    {
        var product = _mongoDBContext.Products.Find(p => p.Id == id).FirstOrDefault();

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    // POST: api/products
    [HttpPost]
    public IActionResult PostProduct([FromBody] Product product)
    {
        _mongoDBContext.Products.InsertOne(product);

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    // PUT: api/products/5
    [HttpPut("{id}")]
    public IActionResult PutProduct(string id, [FromBody] Product updatedProduct)
    {
        var product = _mongoDBContext.Products.FindOneAndUpdate(
            Builders<Product>.Filter.Eq(p => p.Id, id),
            Builders<Product>.Update
                .Set(p => p.Name, updatedProduct.Name)
                .Set(p => p.Price, updatedProduct.Price),
            new FindOneAndUpdateOptions<Product> { ReturnDocument = ReturnDocument.After }
        );

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    // DELETE: api/products/5
    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(string id)
    {
        var result = _mongoDBContext.Products.DeleteOne(p => p.Id == id);

        if (result.DeletedCount == 0)
        {
            return NotFound();
        }

        return NoContent();
    }
}
