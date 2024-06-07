using MQTTnet;
using MQTTnet.Client;
using System.Globalization;
using System.Text.Json;
using System.Text;
using EventInfo.Models;
using MQTTnet.Server;

namespace EventInfo.Service
{
    public class MqttService
    {
        private readonly IMqttClient mqttClient;
        private readonly MqttClientOptions mqttOptions;
        private readonly List<WeatherData> receivedMessages = new List<WeatherData>();

        public MqttService()
        {
            var factory = new MqttFactory();
            mqttClient = factory.CreateMqttClient();

            mqttOptions = new MqttClientOptionsBuilder()
                .WithClientId("EventInfo")
                .WithTcpServer("mosquitto", 8883)
                .WithCleanSession()
                .Build();


            mqttClient.ConnectedAsync += async e =>
            {
                await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("analytics/weather-data/+").Build());
                Console.WriteLine("Connected successfully with MQTT broker.");
            };

            mqttClient.DisconnectedAsync += async e =>
            {
                Console.WriteLine("Disconnected from MQTT broker.");
                await Task.Delay(TimeSpan.FromSeconds(5));
                try
                {
                    await mqttClient.ConnectAsync(mqttOptions, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Reconnect failed: {ex.Message}");
                }
            };

            mqttClient.ApplicationMessageReceivedAsync += async e =>
            {

                var message = e.ApplicationMessage.Payload;

                WeatherData receivedData = JsonSerializer.Deserialize<WeatherData>(message, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (receivedData != null)
                {
                    receivedMessages.Add(receivedData);
                    Console.WriteLine(receivedData.AirTemperature);
                }
                else
                {
                    Console.WriteLine("Deserialization failed.");
                }
            };
            mqttClient.ConnectAsync(mqttOptions).Wait();
        }

        public List<WeatherData> GetData()
        {
            return receivedMessages;
        }
    }
}
