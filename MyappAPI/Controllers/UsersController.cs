using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyappAPI.Models;
using MyappAPI.DTO;
using NuGet.Protocol.Core.Types;

namespace MyappAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserContext _context;

        public UsersController(UserContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(long id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        //GET Number of month
        [HttpGet("numberMonth/{id}")]
        public async Task<ActionResult<User>> GetMonth(long id)
        {
            var user = await _context.Users.FindAsync(id);
            DateTime birth = user.birth;

            int month = CalculateMonth(birth);

            if (month < 0)
            {
                var response = new { message = "Votre anniversaire est déja passé de " + (month) + " mois"};
                return Ok(response);
            }
            else if (month == 0)
            {
                var response = new { message = "Félicitation c'est votre mois d'anniversaire" };
                return Ok(response);
            }
            else
            {
                var response = new { message = "Votre anniversaire sera dans " + month + " mois"};
                return Ok(response);
            }

        }

        //Calculate month
        private int CalculateMonth(DateTime birth)
        {
            DateTime today = DateTime.Today;
            int month = today.Month - birth.Month;

            return month;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                name = userDTO.name,
                birth = userDTO.birth,
                age = CalculateBirth(userDTO.birth)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        private int CalculateBirth(DateTime birth)
        {
            DateTime today = DateTime.Today;

            int userAge = today.Year - birth.Year;

            return userAge;
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
