using CRUD.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize("Bearer", Roles = "admin")]
    public class TimeController : Controller
    {
        private readonly TimeContext _context;

        public TimeController(TimeContext timeContext)
        {
            _context = timeContext;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Time>))]
        public async Task<IActionResult> Get()
        {           
            var times = await _context.Times
                .AsNoTracking()
                .ToListAsync();

            return Ok(times);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Time))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var time = await _context.Times
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (time == null)
            {
                return NotFound();
            }            

            return Ok(time);
        }

        [HttpGet("{id}/players")]
        [ProducesResponseType(200, Type = typeof(List<Jogador>))]
        public async Task<IActionResult> GetPlayers([FromRoute] int id)
        {
            var players = await _context.Jogadores
                .AsNoTracking()
                .Where(m => m.TimeId == id)
                .ToListAsync();

            return Ok(players);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Time))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody]Time time)
        {
            if (ModelState.IsValid)
            {
                _context.Times.Add(time);

                await _context.SaveChangesAsync();

                return Created($"api/time/{time.Id}", time);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(Time))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody]Time time)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var _time = await _context.Times.FirstOrDefaultAsync(x => x.Id == id);

            if (_time != null)
            {
                if (time.Bandeira != null)
                {
                    _time.Bandeira = time.Bandeira;
                }                
                _time.NMTecnico = time.NMTecnico;
                _time.Pais = time.Pais;

                _context.Update(_time);

                await _context.SaveChangesAsync();

                return Ok(_time);
            }
            else
            {
                return NotFound();
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var _time = await _context.Times
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (_time != null)
            {
                if (!await _context.Jogadores.AnyAsync(x => x.TimeId == id))
                {
                    _context.Times.Remove(_time);

                    await _context.SaveChangesAsync();

                    return NoContent();
                }
                else
                {
                    return BadRequest("Não foi possível excluir esse time, pois existem jogadores cadastrados.");
                }
            }
            else
            {
                return NotFound();
            }
        }
    }
}
