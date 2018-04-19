using CRUD.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRUD.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize("Bearer", Roles = "admin")]
    public class PlayerController : Controller
    {
        private readonly TimeContext _context;

        public PlayerController(TimeContext context)
        {
            _context = context;
        }

        // GET: api/Player
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Jogador>))]
        public async Task<IActionResult> Get()
        {
            var players = await _context.Jogadores
                .AsNoTracking()
                .ToListAsync();

            return Ok(players);
        }

        // GET: api/Player/5
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Jogador))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var jogador = await _context.Jogadores
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == id);

            if (jogador == null)
            {
                return NotFound();
            }

            return Ok(jogador);
        }

        // PUT: api/Player/5
        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(Jogador))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Jogador jogador)
        {
            if (!await _context.Jogadores.AnyAsync(x => x.Id == id))
            {
                return NotFound();
            }

            jogador.Id = id;

            if (ModelState.IsValid)
            {
                _context.Jogadores.Attach(jogador).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(jogador);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // POST: api/Player
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Jogador))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody] Jogador jogador)
        {
            if (ModelState.IsValid)
            {
                _context.Jogadores.Add(jogador);
                await _context.SaveChangesAsync();

                return Created($"api/player/{jogador.Id}", jogador);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/Player/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]        
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var jogador = await _context.Jogadores.SingleOrDefaultAsync(m => m.Id == id);
            if (jogador != null)
            {
                _context.Jogadores.Remove(jogador);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
    }
}