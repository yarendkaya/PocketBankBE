using Microsoft.AspNetCore.Mvc;
using PocketBankBE.DTOs;
using PocketBankBE.Models;
using PocketBankBE.Services;

namespace PocketBankBE.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly TransactionService _service;

    public TransactionController(TransactionService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_service.GetAll());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var transaction = _service.GetById(id);
        if (transaction == null) return NotFound();
        return Ok(transaction);
    }

    [HttpPost]
    public IActionResult Add([FromBody] CreateTransactionDto transactionDto)
    {
        var transaction = new Transaction
        {
            AccountId = transactionDto.AccountId,
            Amount = transactionDto.Amount,
            Date = transactionDto.Date,
            Category = transactionDto.Category,
            Description = transactionDto.Description,
            IsIncome = transactionDto.IsIncome
        };

        _service.Add(transaction);
        return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] UpdateTransactionDto transactionDto)
    {
        var existingTransaction = _service.GetById(id);
        if (existingTransaction == null) return NotFound();

        existingTransaction.AccountId = transactionDto.AccountId;
        existingTransaction.Amount = transactionDto.Amount;
        existingTransaction.Date = transactionDto.Date;
        existingTransaction.Category = transactionDto.Category;
        existingTransaction.Description = transactionDto.Description;
        existingTransaction.IsIncome = transactionDto.IsIncome;

        _service.Update(existingTransaction);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _service.Delete(id);
        return NoContent();
    }
}