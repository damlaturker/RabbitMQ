using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebbitMQ_Receive
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "Damla",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Messages person = JsonConvert.DeserializeObject<Messages>(message);
                    Console.WriteLine($" Name: {person.Name} Surname:{person.SurName} [{person.Message}]");
                };
                channel.BasicConsume(queue: "Damla",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine("Mesaj Delivered");
                Console.ReadLine();
            }
        }
    }
    public class Messages
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Message { get; set; }
    }
}
