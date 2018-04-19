using CRUD___WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Wangkanai.Detection;
using WebSite.Token;

namespace CRUD___WebAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IToken _token;
        private readonly string path;
        private readonly IDevice _device;

        public HomeController(IConfiguration configuration, IToken Token, IDeviceResolver deviceResolver)
        {
            _config = configuration;
            _token = Token;
            _device = deviceResolver.Device;

            path = _config.GetValue<string>("API") + "/api";
        }

        public async Task<IActionResult> Index()
        {
            using (var http = new HttpClient())
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.Token);

                var timesContent = await http.GetAsync(path + "/time");

                ICollection<Time> times = JsonConvert.DeserializeObject<ICollection<Time>>(await timesContent.Content.ReadAsStringAsync());

                if (!string.IsNullOrEmpty((string)TempData["MsgError"]))
                {
                    ViewBag.MsgError = TempData["MsgError"];
                    TempData.Remove("MsgError");
                }
                
                return View(times);
            }            
        }

        [HttpGet]
        public IActionResult NewTime()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewTime(Time time, IFormFile Bandeira)
        {
            using (var http = new HttpClient())
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.Token);

                using (var bandeira = new MemoryStream())
                {
                    await Bandeira.CopyToAsync(bandeira);

                    time.Bandeira = bandeira.ToArray();
                }

                var response = await http.PostAsync(path + "/time", new StringContent(JsonConvert.SerializeObject(time), Encoding.UTF8, "application/json"));

                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            using (var http = new HttpClient())
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.Token);

                var response = await http.DeleteAsync(path + $"/time/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var error = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
                    
                    TempData["MsgError"] = error;
                }

                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            using (var http = new HttpClient())
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.Token);

                var response = await http.GetAsync(path + $"/time/{id}");

                Time time = JsonConvert.DeserializeObject<Time>(await response.Content.ReadAsStringAsync());

                return View(time);
            }            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Time time, IFormFile Bandeira)
        {
            using (var http = new HttpClient())
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.Token);

                if (Bandeira != null)
                {
                    using (var bandeira = new MemoryStream())
                    {
                        await Bandeira.CopyToAsync(bandeira);

                        time.Bandeira = bandeira.ToArray();
                    }
                }

                var response = await http.PutAsync(path + $"/time/{time.Id}", new StringContent(JsonConvert.SerializeObject(time), Encoding.UTF8, "application/json"));

                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            if (!string.IsNullOrEmpty((string)TempData["MsgError"]))
            {
                ViewBag.MsgError = TempData["MsgError"];
                TempData.Remove("MsgError");
            }

            using (var http = new HttpClient())
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.Token);

                var response = await http.GetAsync(path + $"/time/{id}");
                var reponsePlayer = await http.GetAsync(path + $"/time/{id}/players");

                Time time = JsonConvert.DeserializeObject<Time>(await response.Content.ReadAsStringAsync());

                time.Jogadores = JsonConvert.DeserializeObject<ICollection<Jogador>>(await reponsePlayer.Content.ReadAsStringAsync());

                foreach (var jogador in time.Jogadores)
                {
                    var responseFicha = await http.GetAsync(path + $"/ficha/{jogador.Id}");

                    jogador.Ficha = JsonConvert.DeserializeObject<Ficha>(await responseFicha.Content.ReadAsStringAsync());
                }

                ViewBag.IsMobile = _device.Type == DeviceType.Mobile;
                return View(time);
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
