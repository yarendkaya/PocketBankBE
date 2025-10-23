using Microsoft.AspNetCore.Mvc;
using PocketBankBE.DTOs;
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
    public IActionResult Add([FromBody] CreateSavingGoalDto goalDto)
    {
        var goal = new SavingGoal
        {
            GoalName = goalDto.GoalName,
            TargetAmount = goalDto.TargetAmount,
            CurrentAmount = goalDto.CurrentAmount,
            TargetDate = goalDto.TargetDate
            // UserId should be set based on authenticated user
        };

        _service.Add(goal);
        return CreatedAtAction(nameof(GetById), new { id = goal.Id }, goal);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] UpdateSavingGoalDto goalDto)
    {
        var existingGoal = _service.GetById(id);
        if (existingGoal == null) return NotFound();

        existingGoal.GoalName = goalDto.GoalName;
        existingGoal.TargetAmount = goalDto.TargetAmount;
        existingGoal.CurrentAmount = goalDto.CurrentAmount;
        existingGoal.TargetDate = goalDto.TargetDate;

        _service.Update(existingGoal);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _service.Delete(id);
        return NoContent();
    }
}