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
    public class NewGame
    {
        [DisplayName("Time 1")]
        public int Time1 { get; set; }

        [DisplayName("Time 2")]
        public int Time2 { get; set; }

        [DisplayName("Data do jogo")]
        public string Data { get; set; }

        public List<SelectListItem> Times { get; } = new List<SelectListItem>();

        public async Task LoadTimes(IConfiguration configuration, IToken Token)
        {
            var path = configuration.GetValue<string>("API") + "/api";

            using (var http = new HttpClient())
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token.Token);

                var timesContent = await http.GetAsync(path + "/time");

                ICollection<Time> times = JsonConvert.DeserializeObject<ICollection<Time>>(await timesContent.Content.ReadAsStringAsync());

                foreach (var time in times)
                {
                    Times.Add(new SelectListItem() { Value = time.Id.ToString(), Text = time.Pais });
                }
            }
        }
    }
}
