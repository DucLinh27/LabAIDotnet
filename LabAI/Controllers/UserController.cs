// Controllers/UsersController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using LabAI.Models;
using LabAI.Data;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/users
    [HttpGet]
    public IActionResult GetUsers()
    {
        var users = _context.Users.ToList();
        return Ok(users);
    }

    // GET: api/users/5
    [HttpGet("{id}")]
    public IActionResult GetUser(int id)
    {
        var user = _context.Users
            .FirstOrDefault(u => u.Id == id);

        if (user == null)
        {
            return NotFound();
        }

        // Store UserId in a cookie
        Response.Cookies.Append("UserId", user.Id.ToString(), new CookieOptions
        {
            Expires = DateTimeOffset.Now.AddMinutes(10), // Set the expiration time as needed
            HttpOnly = true,
            Secure = true, // For secure environments (requires HTTPS)
            SameSite = SameSiteMode.Strict
        });

        return Ok(user);
    }

    // POST: api/users
    [HttpPost]
    public IActionResult PostUser([FromBody] User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    // PUT: api/users/5
    [HttpPut("{id}")]
    public IActionResult PutUser(int id, [FromBody] User updatedUser)
    {
        var user = _context.Users.Find(id);

        if (user == null)
        {
            return NotFound();
        }

        user.Username = updatedUser.Username;
        user.Password = updatedUser.Password;

        _context.SaveChanges();

        return NoContent();
    }

    // DELETE: api/users/5
    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        var user = _context.Users.Find(id);

        if (user == null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);
        _context.SaveChanges();

        return NoContent();
    }
}
