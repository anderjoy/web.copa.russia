using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebSite.Token
{
    public class Token : IToken
    {
        private string _token;
        private readonly IConfiguration _config;

        string IToken.Token => _token;

        public Token(IConfiguration configuration)
        {
            _config = configuration;

            var task = Task.Run(async () => await LoadToken());
            task.Wait();
        }

        private async Task LoadToken()
        {
            using (HttpClient http = new HttpClient())
            {
                string path = _config.GetValue<string>("API") + "/api";

                var user = new { UserID = "ajesus", AccessKey = "ajesus" };

                var response = await http.PostAsync(path + "/login", new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));

                var token = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());

                _token = token.accessToken;
            }
        }
    }
}
