using Microsoft.AspNetCore.Mvc;
using MvcWebApiSwaggerApp.Models;

[ApiController]
[Route("api/dbtest")]
public class DbTestApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public DbTestApiController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult Insert()
    {
        var data = new DbTest
        {
            Name = "Manual DB Connection Working"
        };

        _context.DbTest.Add(data);
        _context.SaveChanges();

        return Ok("Inserted Successfully");
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_context.DbTest.ToList());
    }
}
