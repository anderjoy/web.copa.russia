using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Wangkanai.Detection;
using WebSite.Token;

namespace CRUD___WebAPI.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IToken _token;
        private readonly string path;
        private readonly IDevice _device;

        public PlayerController(IConfiguration configuration, IToken Token, IDeviceResolver deviceResolver)
        {
            _config = configuration;
            _token = Token;
            _device = deviceResolver.Device;

            path = _config.GetValue<string>("API") + "/api";
        }

        [HttpGet]
        public async Task<IActionResult> NewPlayer(int id)
        {
            using (var http = new HttpClient())
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.Token);

                var response = await http.GetAsync(path + $"/time/{id}");

                Time time = JsonConvert.DeserializeObject<Time>(await response.Content.ReadAsStringAsync());

                ViewBag.TimeId = time.Id;
                ViewBag.Bandeira = time.Bandeira;

                ViewBag.IsMobile = _device.Type == DeviceType.Mobile;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewPlayer(Jogador jogador)
        {
            using (var http = new HttpClient())
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.Token);

                Ficha _ficha = jogador.Ficha;

                jogador.Id = 0;
                jogador.Ficha = null;

                var response = await http.PostAsync(path + "/player", new StringContent(JsonConvert.SerializeObject(jogador), System.Text.Encoding.UTF8, "application/json"));

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    var _newJogador = JsonConvert.DeserializeObject<Jogador>(await response.Content.ReadAsStringAsync());

                    _ficha.JogadorId = _newJogador.Id;
                    _ficha.Altura = _ficha.Altura / 100;

                    var responseFicha = await http.PostAsync(path + "/ficha", new StringContent(JsonConvert.SerializeObject(_ficha), System.Text.Encoding.UTF8, "application/json"));

                    return RedirectToAction("Detail", "Home", new { id = jogador.TimeId });
                }
                else
                {
                    ViewBag.MsgError = await response.Content.ReadAsStringAsync();
                    return RedirectToAction("NewPlayer");
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            using (var http = new HttpClient())
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.Token);

                var responsePlayer = await http.GetAsync($"{path}/player/{id}");
                var responseFicha = await http.GetAsync($"{path}/ficha/{id}");

                var _player = JsonConvert.DeserializeObject<Jogador>(await responsePlayer.Content.ReadAsStringAsync());
                _player.Ficha = JsonConvert.DeserializeObject<Ficha>(await responseFicha.Content.ReadAsStringAsync());

                var responseTime = await http.GetAsync(path + $"/time/{_player.TimeId}");

                Time time = JsonConvert.DeserializeObject<Time>(await responseTime.Content.ReadAsStringAsync());

                ViewBag.TimeId = time.Id;
                ViewBag.Bandeira = time.Bandeira;
                ViewBag.IsMobile = _device.Type == DeviceType.Mobile;
                return View(_player);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Jogador jogador)
        {
            using (var http = new HttpClient())
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.Token);

                Ficha _ficha = jogador.Ficha;

                jogador.Ficha = null;

                var response = await http.PutAsync($"{path}/player/{jogador.Id}", new StringContent(JsonConvert.SerializeObject(jogador), System.Text.Encoding.UTF8, "application/json"));

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {                 
                    _ficha.JogadorId = jogador.Id;
                    _ficha.Altura = _ficha.Altura / 100;

                    var responseFicha = await http.PutAsync($"{path}/ficha/{jogador.Id}", new StringContent(JsonConvert.SerializeObject(_ficha), System.Text.Encoding.UTF8, "application/json"));

                    return RedirectToAction("Detail", "Home", new { id = jogador.TimeId });
                }
                else
                {
                    ViewBag.MsgError = await response.Content.ReadAsStringAsync();
                    return RedirectToAction("NewPlayer");
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, int timeid)
        {
            using (var http = new HttpClient())
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.Token);

                var responseFicha = await http.DeleteAsync(path + $"/ficha/{id}");
                var response = await http.DeleteAsync(path + $"/player/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var error = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());

                    TempData["MsgError"] = error;
                }

                return RedirectToAction("Detail", "Home", new { id = timeid });
            }
        }
    }
}