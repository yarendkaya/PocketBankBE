using Microsoft.AspNetCore.Mvc;
using PocketBankBE.Models;
using PocketBankBE.Services;

namespace PocketBankBE.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BudgetController : ControllerBase
{
    private readonly BudgetService _service;

    public BudgetController(BudgetService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll() => Ok(_service.GetAll());

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var budget = _service.GetById(id);
        if (budget == null) return NotFound();
        return Ok(budget);
    }

    [HttpPost]
    public IActionResult Add(Budget budget)
    {
        _service.Add(budget);
        return CreatedAtAction(nameof(GetById), new { id = budget.Id }, budget);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Budget budget)
    {
        if (id != budget.Id) return BadRequest();
        _service.Update(budget);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _service.Delete(id);
        return NoContent();
    }
}