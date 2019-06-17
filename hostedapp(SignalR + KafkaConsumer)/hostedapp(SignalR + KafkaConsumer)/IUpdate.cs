using System.Threading.Tasks;

namespace hostedapp_SignalR___KafkaConsumer_
{
    public interface IUpdate
    {
        Task sendAsync(string v, int value);
    }
}