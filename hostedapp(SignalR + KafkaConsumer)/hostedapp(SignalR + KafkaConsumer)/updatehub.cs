using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace hostedapp_SignalR___KafkaConsumer_
{
    public class Updatehub : Hub
    {
        public async Task SendAsync(int value)
        {
            await Clients.All.SendAsync("ReceiveMessage", value);
        }
    }
}