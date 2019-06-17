using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Producer
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            // If serializers are not specified, default serializers from
            // `Confluent.Kafka.Serializers` will be automatically used where
            // available. Note: by default strings are encoded as UTF8.
            using (var p = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    Console.WriteLine("Producer app is producing");
                    while (true)
                    {
                        Random rnd = new Random();
                        POCO obj = new POCO();
                        obj.counter= rnd.Next(0,10000);
                        string s = JsonConvert.SerializeObject(obj);
                        var dr = await p.ProduceAsync("demochannel", new Message<Null, string> { Value = s });
                        Thread.Sleep(30000);
                    }
                    
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
            }
        }
    }
}
