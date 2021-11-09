using System.Linq;
using Core.Models;
using DataStore.EF;
using Microsoft.AspNetCore.Mvc;
using PlatformDemo.ActionFilter;

namespace PlatformDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly BugsContext db;

        public ProjectsController(BugsContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(db.Projects.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id )
        {
            var project = db.Projects.Find(id);

            if (project == null)
                return NotFound();

            return Ok(project);
        }

        [HttpGet]
        [Route("/api/projects/{pid}/tickets")]
        public IActionResult GetProjectTickets(int pid)
        {
            var tickets = db.Tickets.Where(t => t.ProjectId == pid).ToList();


            if (tickets.Count > 0)
            {
                return Ok(tickets);
            }

            else return NotFound();
        }

        [HttpPost]
        public IActionResult Create([FromBody] Project project)
        {
            if (db.Projects.Find(project.ProjectId) != null)
            {
                return BadRequest("ProjectId already in use");
            }
            
            var entityProject = db.Projects.Add(project);
            db.SaveChanges();

            return CreatedAtAction(nameof(GetById),
                new { id = project.ProjectId },
                project

                );
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Project project)
        {
            if (id != project.ProjectId) return BadRequest();

            db.Entry(project).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch 
            {
                if (db.Projects.Find(id) == null)
                {
                    return NotFound();
                }
                throw;
            }
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) 
        {
            var project = db.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            db.Projects.Remove(project);
            db.SaveChanges();

            return Ok();
        }
    }

}
