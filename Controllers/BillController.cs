using Microsoft.AspNetCore.Mvc;
using PocketBankBE.Models;
using PocketBankBE.Services;

namespace PocketBankBE.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BillController : ControllerBase
{
    private readonly BillService _service;

    public BillController(BillService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll() => Ok(_service.GetAll());

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var bill = _service.GetById(id);
        if (bill == null) return NotFound();
        return Ok(bill);
    }

    [HttpPost]
    public IActionResult Add(Bill bill)
    {
        _service.Add(bill);
        return CreatedAtAction(nameof(GetById), new { id = bill.Id }, bill);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Bill bill)
    {
        if (id != bill.Id) return BadRequest();
        _service.Update(bill);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _service.Delete(id);
        return NoContent();
    }
}