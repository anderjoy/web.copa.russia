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
    public class FichaController : Controller
    {
        private readonly TimeContext _context;

        public FichaController(TimeContext context)
        {
            _context = context;
        }

        // GET: api/Ficha
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Ficha>))]
        public async Task<IActionResult> Get()
        {
            var fichas = await _context.Fichas
                .AsNoTracking()
                .ToListAsync();

            return Ok(fichas);
        }

        // GET: api/Ficha/5
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Ficha))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var ficha = await _context.Fichas
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.JogadorId == id);

            if (ficha == null)
            {
                return NotFound();
            }

            return Ok(ficha);
        }

        // PUT: api/Ficha/5
        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(Ficha))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Ficha ficha)
        {
            if (!await _context.Fichas.AnyAsync(x => x.JogadorId == id))
            {
                return NotFound();
            }

            ficha.JogadorId = id;

            if (ModelState.IsValid)
            {
                _context.Fichas.Attach(ficha).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(ficha);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // POST: api/Ficha
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Ficha))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody] Ficha ficha)
        {
            if (ModelState.IsValid)
            {
                _context.Fichas.Add(ficha);
                await _context.SaveChangesAsync();

                return Created($"api/ficha/{ficha.JogadorId}", ficha);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/Ficha/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var ficha = await _context.Fichas
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.JogadorId == id);

            if (ficha == null)
            {
                return NotFound();
            }
            else
            {
                _context.Fichas.Remove(ficha);
                await _context.SaveChangesAsync();

                return NoContent();
            }
        }
    }
}