using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUD.Context;
using Models;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize("Bearer", Roles = "admin")]
    public class GameController : Controller
    {
        private readonly TimeContext _context;

        public GameController(TimeContext context)
        {
            _context = context;
        }

        // GET: api/Game
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Jogo>))]
        public async Task<IActionResult> GetJogo()
        {
            var jogos = await _context.Jogos
                .Include(x => x.Time_1)
                .Include(x => x.Time_2)
                .AsNoTracking()
                .ToListAsync();

            return Ok(jogos);
        }

        // GET: api/Game/5
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Jogo))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetJogo([FromRoute] int id)
        {
            var jogo = await _context.Jogos
                .Include(x => x.Time_1)
                .Include(x => x.Time_2)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (jogo == null)
            {
                return NotFound();
            }

            return Ok(jogo);
        }

        [HttpGet("{id}/goals")]
        [ProducesResponseType(200, Type = typeof(List<Gol>))]
        public async Task<IActionResult> GetGoals([FromRoute] int id)
        {
            var gols = await _context.Gols
                .Include(x => x.Jogador)
                .Include(x => x.Time)
                .AsNoTracking()                
                .Where(x => x.JogoId == id)
                .OrderBy(x => x.Hora)
                .ToListAsync();

            return Ok(gols);
        }

        // PUT: api/Game/5
        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(Jogo))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutJogo([FromRoute] int id, [FromBody] Jogo jogo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _context.Jogos.AnyAsync(x => x.Id == id))
            {
                jogo.Id = id;

                _context.Attach(jogo).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return NoContent();
            }

            return NotFound();
        }

        // POST: api/Game
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Jogo))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostJogo([FromBody] Jogo jogo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Jogos.Add(jogo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJogo", new { id = jogo.Id }, jogo);
        }

        // DELETE: api/Game/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteJogo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var jogo = await _context.Jogos.SingleOrDefaultAsync(m => m.Id == id);
            if (jogo == null)
            {
                return NotFound();
            }

            _context.Jogos.Remove(jogo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}