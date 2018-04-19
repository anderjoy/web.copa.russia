using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models;
using Newtonsoft.Json;
using WebSite.Token;

namespace WebSite.Controllers
{
    [Authorize]
    public class GoalController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IToken _token;
        private readonly string path;

        public GoalController(IConfiguration configuration, IToken Token)
        {
            _config = configuration;
            _token = Token;

            path = _config.GetValue<string>("API") + "/api";
        }

        [Route("Game/{id}/Goals")]
        public async Task<IActionResult> Index(int id)
        {
            using (var http = new HttpClient())
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.Token);

                var response = await http.GetAsync($"{path}/game/{id}/goals");
                List<Gol> gols = JsonConvert.DeserializeObject<List<Gol>>(await response.Content.ReadAsStringAsync());

                var responseGame = await http.GetAsync($"{path}/game/{id}");
                Jogo jogo = JsonConvert.DeserializeObject<Jogo>(await responseGame.Content.ReadAsStringAsync());

                int totalGolTime1 = 0;
                int totalGolTime2 = 0;

                foreach (var item in gols.OrderBy(x => x.TimeId))
                {
                    if (item.TimeId == jogo.Time_1.Id)
                    {
                        totalGolTime1++;
                    }
                    else
                    {
                        totalGolTime2++;
                    }
                }

                ViewBag.Time1 = jogo.Time_1.Pais;
                ViewBag.Time2 = jogo.Time_2.Pais;
                ViewBag.GameID = id;
                ViewBag.TotalGolTime1 = totalGolTime1;
                ViewBag.TotalGolTime2 = totalGolTime2;
                return View(gols);
            }
        }

        [HttpGet]
        [Route("Game/{id}/NewGoal")]
        public async Task<IActionResult> NewGoal(int id)
        {
            ViewModel.NewGoal newGoal = new ViewModel.NewGoal();
            await newGoal.LoadCombos(_config, _token, id);

            ViewBag.GameID = id;
            return View(newGoal);
        }

        [HttpGet]
        public async Task<JsonResult> Jogadores(int timeId)
        {
            using (var http = new HttpClient())
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.Token);

                var jogadoresContent = await http.GetAsync($"{path}/time/{timeId}/players");
                var jogadores = JsonConvert.DeserializeObject<ICollection<Jogador>>(await jogadoresContent.Content.ReadAsStringAsync());

                return Json(jogadores.ToList());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Game/{gameId}/NewGoal")]
        public async Task<IActionResult> NewGoal(ViewModel.NewGoal newGoal, int gameId)
        {
            if (!TimeSpan.TryParse(newGoal.Hora, out TimeSpan resultado))
            {
                await newGoal.LoadCombos(_config, _token, gameId);

                ViewBag.MsgError = "Hora inválida.";
                ViewBag.GameID = gameId;
                return View(newGoal);
            }

            using (var http = new HttpClient())
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.Token);

                Gol gol = new Gol()
                {
                    JogadorId = newGoal.Jogador,
                    TimeId = newGoal.Time,
                    JogoId = gameId,
                    Hora = TimeSpan.Parse(newGoal.Hora)
                };

                var response = await http.PostAsync($"{path}/goal", new StringContent(JsonConvert.SerializeObject(gol), Encoding.UTF8, "application/json"));

                return RedirectToAction("Index", new { id = gameId });
            }
        }
    }
}