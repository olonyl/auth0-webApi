using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIApplication.Models;

namespace WebAPIApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmploymentsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public EmploymentsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Employments
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Employment>>> GetEmployments()
        {
            // Retrieve the access_token claim which we saved in the OnTokenValidated event
            string accessToken = User.Claims.FirstOrDefault(c => c.Type == "access_token").Value;
            return await _context.Employments.ToListAsync();
        }

        // GET: api/Employments/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Employment>> GetEmployment(int id)
        {
            var employment = await _context.Employments.FindAsync(id);

            if (employment == null)
            {
                return NotFound();
            }

            return employment;
        }

        // PUT: api/Employments/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutEmployment(int id, Employment employment)
        {
            if (id != employment.Id)
            {
                return BadRequest();
            }

            _context.Entry(employment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmploymentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employments
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Employment>> PostEmployment(Employment employment)
        {
            _context.Employments.Add(employment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployment", new { id = employment.Id }, employment);
        }

        // DELETE: api/Employments/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Employment>> DeleteEmployment(int id)
        {
            var employment = await _context.Employments.FindAsync(id);
            if (employment == null)
            {
                return NotFound();
            }

            _context.Employments.Remove(employment);
            await _context.SaveChangesAsync();

            return employment;
        }

        private bool EmploymentExists(int id)
        {
            return _context.Employments.Any(e => e.Id == id);
        }
    }
}
