using System;
using AutoMapper;
using Core.Models;
using DataStore.EF;
using Microsoft.AspNetCore.Mvc;
using PlatformDemo.DTO;

namespace PlatformDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("2.0")]
    public class TicketsController : ControllerBase
    {
        private readonly BugsContext context;
        private readonly IMapper _mapper;

        public TicketsController(BugsContext context, IMapper mapper)
        {
            this.context = context;
            _mapper = mapper;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(context.Tickets);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id )
        {
            var ticket = context.Find<Ticket>(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<TicketDTO>(ticket));
        }

        [HttpPost]
        public IActionResult Create([FromBody] Ticket ticket)
        {   
            var entityProject = context.Tickets.Add(ticket);
            context.SaveChanges();

            return CreatedAtAction(nameof(Get),
                new { id = ticket.ProjectId },
                ticket

                );
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Ticket ticket)
        {
            if (id != ticket.TicketId) return BadRequest();

            if (context.Find<Ticket>(ticket.TicketId) == null)
            {
                return NotFound();
            }

            context.Tickets.Update(ticket);
            context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) 
        {
            if (context.Find<Ticket>(id) == null)
            {
                return NotFound();
            }

            context.Tickets.Remove(context.Find<Ticket>(id));
            context.SaveChanges();

            return Ok();
        }
    }

}
