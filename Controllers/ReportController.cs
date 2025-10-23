using Microsoft.AspNetCore.Mvc;
using PocketBankBE.Models;
using PocketBankBE.Services;

namespace PocketBankBE.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly ReportService _service;

    public ReportController(ReportService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll() => Ok(_service.GetAll());

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var report = _service.GetById(id);
        if (report == null) return NotFound();
        return Ok(report);
    }

    [HttpPost]
    public IActionResult Add(Report report)
    {
        _service.Add(report);
        return CreatedAtAction(nameof(GetById), new { id = report.Id }, report);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Report report)
    {
        if (id != report.Id) return BadRequest();
        _service.Update(report);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _service.Delete(id);
        return NoContent();
    }
}