// Controllers/AddressesController.cs
using LabAI.Data;
using LabAI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

[Route("api/[controller]")]
[ApiController]
public class AddressesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AddressesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/addresses
    [HttpGet]
    public IActionResult GetAddresses()
    {
        var addresses = _context.Addresses.ToList();
        return Ok(addresses);
    }

    // GET: api/addresses/5
    [HttpGet("{id}")]
    public IActionResult GetAddress(int id)
    {
        var address = _context.Addresses.Find(id);

        if (address == null)
        {
            return NotFound();
        }

        return Ok(address);
    }

    // POST: api/addresses
    [HttpPost]
    public IActionResult PostAddress([FromBody] Address address)
    {
        _context.Addresses.Add(address);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetAddress), new { id = address.Id }, address);
    }

    // PUT: api/addresses/5
    [HttpPut("{id}")]
    public IActionResult PutAddress(int id, [FromBody] Address updatedAddress)
    {
        var address = _context.Addresses.Find(id);

        if (address == null)
        {
            return NotFound();
        }

        address.Name = updatedAddress.Name;

        _context.SaveChanges();

        return NoContent();
    }

    // DELETE: api/addresses/5
    [HttpDelete("{id}")]
    public IActionResult DeleteAddress(int id)
    {
        var address = _context.Addresses.Find(id);

        if (address == null)
        {
            return NotFound();
        }

        _context.Addresses.Remove(address);
        _context.SaveChanges();

        return NoContent();
    }
}
