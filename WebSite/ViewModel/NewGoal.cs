using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebSite.Token;

namespace WebSite.ViewModel
{
    public class NewGoal
    {
        [DisplayName("Time")]
        public int Time { get; set; }

        [DisplayName("Jogador")]
        public int Jogador { get; set; }

        [DisplayName("Hora do gol")]
        public string Hora { get; set; }

        public List<SelectListItem> Times { get; } = new List<SelectListItem>();

        public List<SelectListItem> Jogadores { get; } = new List<SelectListItem>();

        public async Task LoadCombos(IConfiguration configuration, IToken Token, int gameId)
        {
            var path = configuration.GetValue<string>("API") + "/api";

            using (var http = new HttpClient())
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token.Token);

                var gameContent = await http.GetAsync($"{path}/game/{gameId}");

                Jogo jogo = JsonConvert.DeserializeObject<Jogo>(await gameContent.Content.ReadAsStringAsync());

                Times.Add(new SelectListItem() { Value = jogo.Time_1.Id.ToString(), Text = jogo.Time_1.Pais });
                Times.Add(new SelectListItem() { Value = jogo.Time_2.Id.ToString(), Text = jogo.Time_2.Pais });

                var jogadoresContent = await http.GetAsync(path + "/player");
                ICollection<Jogador> jogadores = JsonConvert.DeserializeObject<ICollection<Jogador>>(await jogadoresContent.Content.ReadAsStringAsync());

                foreach (var item in jogadores)
                {
                    Jogadores.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.Nome });
                }
            }
        }
    }
}
