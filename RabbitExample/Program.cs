using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Messages message = new Messages() { Name = "Damla", SurName = "Turker", Message = "Hello World" };
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "Borsoft",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string newMessage = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(newMessage);

                channel.BasicPublish(exchange: "",
                                     routingKey: "Damla",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine($"Gönderilen kişi: {message.Name}-{message.SurName}");
            }

            Console.WriteLine("Send Message...");
            Console.ReadLine();
        }
    }
    public class Messages
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Message { get; set; }
    }
}
