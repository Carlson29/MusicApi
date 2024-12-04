using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Models;
using MusicApi.Service;

namespace MusicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ArtistsContext _context;
        private readonly JwtService _jwtService;
        public UsersController(ArtistsContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseModel>> Login(UserDto request)
        {
            var result = await _jwtService.Authenticate(request);
            if (result == null)
            {
                return Unauthorized();

            }
            return result;
        }

      
       /*

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers(int id, Users users)
        {
            if (id != users.Id)
            {
                return BadRequest();
            }

            _context.Entry(users).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(id))
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

      

  

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }*/
    }
}
