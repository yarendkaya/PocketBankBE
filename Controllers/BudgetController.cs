using Microsoft.AspNetCore.Mvc;
using PocketBankBE.DTOs;
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
    public IActionResult Add([FromBody] CreateBudgetDto budgetDto)
    {
        var budget = new Budget
        {
            Name = budgetDto.Name,
            Limit = budgetDto.Limit,
            Period = budgetDto.Period
            // UserId should be set based on authenticated user
        };

        _service.Add(budget);
        return CreatedAtAction(nameof(GetById), new { id = budget.Id }, budget);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] UpdateBudgetDto budgetDto)
    {
        var existingBudget = _service.GetById(id);
        if (existingBudget == null) return NotFound();

        existingBudget.Name = budgetDto.Name;
        existingBudget.Limit = budgetDto.Limit;
        existingBudget.Period = budgetDto.Period;

        _service.Update(existingBudget);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _service.Delete(id);
        return NoContent();
    }
}