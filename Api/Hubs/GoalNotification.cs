using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace API.Hubs
{
    public class GoalNotification : Hub
    {
        public Task SendNewGoal(string Pais, string Jogador)
        {
            return Clients.All.SendAsync("ReceiveNewGoal", Pais, Jogador);
        }
    }
}
