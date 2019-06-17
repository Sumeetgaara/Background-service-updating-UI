using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hostedapp_SignalR___KafkaConsumer_
{
    public class Worker : BackgroundService
    {

        private readonly ILogger<Worker> _logger;
        private readonly IHubContext<Updatehub> _uhub;

        public Worker(ILogger<Worker> logger, IHubContext<Updatehub> uhub)
        {
            _logger = logger;
            _uhub = uhub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var conf = new ConsumerConfig
            {
                GroupId = "test-consumer-group",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var c = new ConsumerBuilder<Ignore, string>(conf).Build())
            {
                c.Subscribe("demochannel");

                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) => {
                    e.Cancel = true; // prevent the process from terminating.
                    cts.Cancel();
                };

                try
                {
                    while (true)
                    {
                        try
                        {
                            Console.WriteLine("background kafka consumer is up");
                            var cr = c.Consume(cts.Token);
                            string receivedMessage = cr.Value;
                            POCO obj = new POCO();
                            obj = JsonConvert.DeserializeObject<POCO>(receivedMessage);
                            await _uhub.Clients.All.SendAsync("ReceiveMessage", obj.counter);
                            await Task.Delay(1000);
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occured: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ensure the consumer leaves the group cleanly and final offsets are committed.
                    c.Close();
                }
            }
        }
    }
}