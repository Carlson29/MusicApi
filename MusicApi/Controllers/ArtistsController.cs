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
    public class ArtistsController : ControllerBase
    {
        private readonly ArtistsContext _context;

        public ArtistsController(ArtistsContext context)
        {
            _context = context;
            context.Database.EnsureCreated();
        }

        // GET: api/Artists
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Artists>>> GetArtists()
        {
            var artist = await _context.Artists.Select(a =>
            new ArtistsDto()
            {
                Artist_Name = a.Artist_Name,
                Bio = a.Bio,
                DateOfBirth = a.DateOfBirth,
                Age = a.getAge()
            }
     ).ToListAsync();
            return Ok(artist);
        }

        // GET: api/Artists/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ArtistsDto>> GetArtists(int id)
        {
            var artists = await _context.Artists.FindAsync(id);

            if (artists == null)
            {
                return NotFound();
            }
            ArtistsDto artistsDto = new ArtistsDto()
            {
                Artist_Name = artists.Artist_Name,
                Bio = artists.Bio,
                DateOfBirth = artists.DateOfBirth,
                Age = artists.getAge()
            };

            return artistsDto;
        }

        // PUT: api/Artists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutArtists(int id, ArtistsDto artists)
        {
            var artist = new Artists { Id = id, Artist_Name = artists.Artist_Name, Bio = artists.Bio, DateOfBirth = artists.DateOfBirth };
            

            _context.Entry(artist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtistsExists(id))
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

        // POST: api/Artists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Artists>> PostArtists(ArtistsDto artists)
        {
            var artist = new Artists { Id = 0, Artist_Name = artists.Artist_Name, Bio = artists.Bio, DateOfBirth = artists.DateOfBirth };
            _context.Artists.Add(artist);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArtists", new { id = artist.Id }, artists);
        }

        // DELETE: api/Artists/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteArtists(int id)
        {
            var artists = await _context.Artists.FindAsync(id);
            if (artists == null)
            {
                return NotFound();
            }

            _context.Artists.Remove(artists);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArtistsExists(int id)
        {
            return _context.Artists.Any(e => e.Id == id);
        }

        internal Task PostArtists(Artists newArtist)
        {
            throw new NotImplementedException();
        }

        [HttpGet("Artists/{id}")]
        public async Task<ActionResult<Artists>> GetArtistsWithId(int id)
        {
            var artists = await _context.Artists.FindAsync(id);

            if (artists == null)
            {
                return NotFound();
            }


            return artists;
        }


    }
}
