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
using WebSite.ViewModel;

namespace WebSite.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IToken _token;
        private readonly string path;

        public GameController(IConfiguration configuration, IToken Token)
        {
            _config = configuration;
            _token = Token;

            path = _config.GetValue<string>("API") + "/api";
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            using (var http = new HttpClient())
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.Token);

                var response = await http.GetAsync(path + "/game");
                var jogos = JsonConvert.DeserializeObject<List<Jogo>>(await response.Content.ReadAsStringAsync());

                return View(jogos);
            }            
        }

        [HttpGet]
        public async Task<IActionResult> NewGame()
        {
            var newGame = new NewGame();
            await newGame.LoadTimes(_config, _token);

            return View(newGame);
        }        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewGame(NewGame NewGame)
        {
            Jogo jogo = new Jogo()
            {
                Data = DateTime.Parse(NewGame.Data),
                Time1 = NewGame.Time1,
                Time2 = NewGame.Time2
            };

            using (var http = new HttpClient())
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.Token);

                var response = await http.PostAsync(path + "/game", new StringContent(JsonConvert.SerializeObject(jogo), Encoding.UTF8, "application/json"));
                
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    return RedirectToAction("Index");
                }

                await NewGame.LoadTimes(_config, _token);
                ViewBag.MsgError = await response.Content.ReadAsStringAsync();
                return View(NewGame);
            }
        }
    }
}