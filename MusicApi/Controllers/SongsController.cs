using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Models;

namespace MusicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly ArtistsContext _context;

        public SongsController(ArtistsContext context)
        {
            _context = context;
            context.Database.EnsureCreated();
        }

        // GET: api/Songs
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<SongsDto>>> GetSongs()
        {
            var songs = await _context.Songs.Select(s=> 
            new SongsDto()
            {
                Title = s.Title,
                Genre = s.Genre,
                Duration = s.Duration,
                ReleaseDate = s.ReleaseDate
            }

            ).ToListAsync();

            return songs;
        }

        // GET: api/Songs/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<SongsDto>> GetSongs(int id)
        {
            var songs = await _context.Songs.FindAsync(id);

            if (songs == null)
            {
                return NotFound();
            }

            SongsDto songsDto = new SongsDto()
            {
                Title = songs.Title,
                Genre = songs.Genre,
                Duration = songs.Duration,
                ReleaseDate = songs.ReleaseDate
            };

            return songsDto;
        }

        // PUT: api/Songs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutSongs(int id, Songs songs)
        {
            if (id != songs.Id)
            {
                return BadRequest();
            }

            _context.Entry(songs).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SongsExists(id))
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

        // POST: api/Songs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Songs>> PostSongs(Songs songs)
        {
            _context.Songs.Add(songs);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSongs", new { id = songs.Id }, songs);
        }

        // DELETE: api/Songs/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteSongs(int id)
        {
            var songs = await _context.Songs.FindAsync(id);
            if (songs == null)
            {
                return NotFound();
            }

            _context.Songs.Remove(songs);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SongsExists(int id)
        {
            return _context.Songs.Any(e => e.Id == id);
        }
    }
}
