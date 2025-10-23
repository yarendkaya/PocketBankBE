using Microsoft.AspNetCore.Mvc;
using PocketBankBE.DTOs;
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
    public IActionResult Add([FromBody] CreateBillDto billDto)
    {
        var bill = new Bill
        {
            BillName = billDto.BillName,
            Amount = billDto.Amount,
            DueDate = billDto.DueDate,
            IsPaid = billDto.IsPaid
            // UserId should be set based on authenticated user
        };

        _service.Add(bill);
        return CreatedAtAction(nameof(GetById), new { id = bill.Id }, bill);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] UpdateBillDto billDto)
    {
        var existingBill = _service.GetById(id);
        if (existingBill == null) return NotFound();

        existingBill.BillName = billDto.BillName;
        existingBill.Amount = billDto.Amount;
        existingBill.DueDate = billDto.DueDate;
        existingBill.IsPaid = billDto.IsPaid;

        _service.Update(existingBill);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _service.Delete(id);
        return NoContent();
    }
}