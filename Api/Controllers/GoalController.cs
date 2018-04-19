using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUD.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Models;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize("Bearer", Roles = "admin")]
    public class GoalController : Controller
    {
        private readonly TimeContext _context;

        public GoalController(TimeContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Gol))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody] Gol gol)
        {
            if (ModelState.IsValid)
            {
                _context.Gols.Add(gol);
                await _context.SaveChangesAsync();

                await NotifyNewGoal(gol.Id);

                return Created($"api/gol/{gol.Id}", gol);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        private async Task NotifyNewGoal(int GolId)
        {
            var gol = _context.Gols
                .Include(x => x.Time)
                .Include(x => x.Jogador)
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == GolId);

            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:65070/GoalNotification/")
                .Build();

            await connection.StartAsync();

            await connection.SendAsync("SendNewGoal", gol.Time.Pais, gol.Jogador.Nome);
        }
    }
}