using Microsoft.AspNetCore.Mvc;
using PocketBankBE.Models;
using PocketBankBE.Services;

namespace PocketBankBE.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SavingGoalController : ControllerBase
{
    private readonly SavingGoalService _service;

    public SavingGoalController(SavingGoalService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll() => Ok(_service.GetAll());

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var goal = _service.GetById(id);
        if (goal == null) return NotFound();
        return Ok(goal);
    }

    [HttpPost]
    public IActionResult Add(SavingGoal goal)
    {
        _service.Add(goal);
        return CreatedAtAction(nameof(GetById), new { id = goal.Id }, goal);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, SavingGoal goal)
    {
        if (id != goal.Id) return BadRequest();
        _service.Update(goal);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _service.Delete(id);
        return NoContent();
    }
}