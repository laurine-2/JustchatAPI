using JustChatAPI.Data;
using JustChatAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;

public class MessageController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public MessageController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost("messages")]
    public IActionResult SaveMessage([FromBody] Message message)
    {
        message.SentAt = DateTime.UtcNow;
        message.ExpirationDate = message.SentAt.AddMonths(2); // Définir la date d'expiration à 2 mois après la date d'envoi

        _dbContext.Messages.Add(message);
        _dbContext.SaveChanges();

        return Ok("Message saved");
    }

    [HttpGet("messages")]
    public IActionResult GetMessages()
    {
        var messages = _dbContext.Messages.ToList();
        return Ok(messages);
    }
}