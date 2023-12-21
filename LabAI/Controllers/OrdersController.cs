// Controllers/OrdersController.cs
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LabAI.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using LabAI.Data;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly MongoDBContext _mongoDBContext;

    public OrdersController(MongoDBContext mongoDBContext)
    {
        _mongoDBContext = mongoDBContext;
    }

    // GET: api/orders
    [HttpGet]
    public IActionResult GetOrders()
    {
        var orders = _mongoDBContext.Orders.Find(_ => true).ToList();
        return Ok(orders);
    }

    // GET: api/orders/5
    [HttpGet("{id}")]
    public IActionResult GetOrder(string id)
    {
        var order = _mongoDBContext.Orders.Find(o => o.Id == id).FirstOrDefault();

        if (order == null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    // POST: api/orders
    [HttpPost]
    public IActionResult PostOrder([FromBody] Order order)
    {
        // Get UserId from the cookie
        if (Request.Cookies.TryGetValue("UserId", out var userId))
        {
            order.UserId = int.Parse(userId);
            _mongoDBContext.Orders.InsertOne(order);
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        return BadRequest("UserId not found in the cookie.");
    }

    // PUT: api/orders/5
    [HttpPut("{id}")]
    public IActionResult PutOrder(string id, [FromBody] Order updatedOrder)
    {
        var order = _mongoDBContext.Orders.FindOneAndUpdate(
            Builders<Order>.Filter.Eq(o => o.Id, id),
            Builders<Order>.Update
                .Set(o => o.UserId, updatedOrder.UserId)
                .Set(o => o.Items, updatedOrder.Items),
            new FindOneAndUpdateOptions<Order> { ReturnDocument = ReturnDocument.After }
        );

        if (order == null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    // DELETE: api/orders/5
    [HttpDelete("{id}")]
    public IActionResult DeleteOrder(string id)
    {
        var result = _mongoDBContext.Orders.DeleteOne(o => o.Id == id);

        if (result.DeletedCount == 0)
        {
            return NotFound();
        }

        return NoContent();
    }
}
