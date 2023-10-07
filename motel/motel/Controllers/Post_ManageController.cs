using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using motel.Data;
using motel.Models.Domain;

namespace motel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Post_ManageController : ControllerBase
    {
        private readonly AppDbContext _context;

        public Post_ManageController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Post_Manage
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post_Manage>>> GetPost_Manage()
        {
          if (_context.Post_Manage == null)
          {
              return NotFound();
          }
            return await _context.Post_Manage.ToListAsync();
        }

        // GET: api/Post_Manage/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Post_Manage>> GetPost_Manage(int id)
        {
          if (_context.Post_Manage == null)
          {
              return NotFound();
          }
            var post_Manage = await _context.Post_Manage.FindAsync(id);

            if (post_Manage == null)
            {
                return NotFound();
            }

            return post_Manage;
        }

        // PUT: api/Post_Manage/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost_Manage(int id, Post_Manage post_Manage)
        {
            if (id != post_Manage.Id)
            {
                return BadRequest();
            }

            _context.Entry(post_Manage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Post_ManageExists(id))
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

        // POST: api/Post_Manage
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Post_Manage>> PostPost_Manage(Post_Manage post_Manage)
        {
          if (_context.Post_Manage == null)
          {
              return Problem("Entity set 'AppDbContext.Post_Manage'  is null.");
          }
            _context.Post_Manage.Add(post_Manage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPost_Manage", new { id = post_Manage.Id }, post_Manage);
        }

        // DELETE: api/Post_Manage/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost_Manage(int id)
        {
            if (_context.Post_Manage == null)
            {
                return NotFound();
            }
            var post_Manage = await _context.Post_Manage.FindAsync(id);
            if (post_Manage == null)
            {
                return NotFound();
            }

            _context.Post_Manage.Remove(post_Manage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Post_ManageExists(int id)
        {
            return (_context.Post_Manage?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
