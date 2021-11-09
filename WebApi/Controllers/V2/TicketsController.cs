using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Models;
using DataStore.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlatformDemo.DTO;
using PlatformDemo.QueryFilter;

namespace PlatformDemo.Controllers
{
    [ApiController]
    [Route("api/tickets")]
    [ApiVersion("2.0")]
    public class TicketsV2Controller : ControllerBase
    {
        private readonly BugsContext context;
        private readonly IMapper _mapper;

        public TicketsV2Controller(BugsContext context, IMapper mapper)
        {
            this.context = context;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] TicketQueryFilter filter)
        {
            IQueryable<Ticket> tickets = context.Tickets;
            if (filter != null)
            {
                if (filter.Description != null)
                {
                    tickets = tickets.Where(x => x.Description.Equals(filter.Description));
                }

                if (filter.Id != null)
                {
                    tickets = tickets.Where(x => x.TicketId == filter.Id);
                }

                if (filter.Title != null)
                {
                    tickets = tickets.Where(x => x.Title.Equals(filter.Title));
                }
            }
            return Ok(await tickets.ToListAsync());
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
