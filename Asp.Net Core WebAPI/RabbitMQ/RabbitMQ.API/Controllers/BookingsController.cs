using Microsoft.AspNetCore.Mvc;
using RabbitMQ.API.Models;
using RabbitMQ.API.Services;

namespace RabbitMQ.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IMessageProducer _messageProducer;
    public static readonly List<Booking> _bookings = new();

    public BookingsController(IMessageProducer messageProducer)
    {
        _messageProducer = messageProducer;
    }

    [HttpPost]
    public IActionResult CreateBooking(Booking booking)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        _bookings.Add(booking);

        _messageProducer.SendingMessage<Booking>(booking);

        return Ok();

    }



}
