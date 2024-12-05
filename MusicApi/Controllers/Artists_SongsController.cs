using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Models;

namespace MusicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Artists_SongsController : ControllerBase
    {
        private readonly ArtistsContext _context;

        public Artists_SongsController(ArtistsContext context)
        {
            _context = context;
        }

        // GET: api/Artists_Songs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Artists_Songs>>> GetArtists_Songs()
        {
            return await _context.Artists_Songs.ToListAsync();
        }

        // GET: api/Artists_Songs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Artists_Songs>> GetArtists_Songs(int id)
        {
            var artists_Songs = await _context.Artists_Songs.FindAsync(id);

            if (artists_Songs == null)
            {
                return NotFound();
            }

            return artists_Songs;
        }

        // PUT: api/Artists_Songs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArtists_Songs(int id, Artists_Songs artists_Songs)
        {
            if (id != artists_Songs.Id)
            {
                return BadRequest();
            }

            _context.Entry(artists_Songs).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Artists_SongsExists(id))
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

        // POST: api/Artists_Songs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Artists_Songs>> PostArtists_Songs(Artists_Songs artists_Songs)
        {
            _context.Artists_Songs.Add(artists_Songs);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArtists_Songs", new { id = artists_Songs.Id }, artists_Songs);
        }

        // DELETE: api/Artists_Songs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtists_Songs(int id)
        {
            var artists_Songs = await _context.Artists_Songs.FindAsync(id);
            if (artists_Songs == null)
            {
                return NotFound();
            }

            _context.Artists_Songs.Remove(artists_Songs);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Artists_SongsExists(int id)
        {
            return _context.Artists_Songs.Any(e => e.Id == id);
        }
    }
}