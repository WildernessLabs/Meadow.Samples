using Meadow;
using Meadow.Units;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace F7Feather_AzureIoTHub.Controllers;

internal class IoTHubMqttController : IIoTHubController
{
    string IOT_HUB_NAME = Secrets.HUB_NAME;
    string IOT_HUB_DEVICE_ID = Secrets.DEVICE_ID;
    string IOT_HUB_SAS_TOKEN = Secrets.SAS_TOKEN;

    IMqttClient mqttClient;

    public bool isAuthenticated { get; private set; }

    public IoTHubMqttController() { }

    public async Task<bool> Initialize()
    {
        try
        {
            Resolver.Log.Info("Create connection factory...");
            var factory = new MqttFactory();

            Resolver.Log.Info("Create MQTT client...");
            mqttClient = factory.CreateMqttClient();

            var iotHubUri = $"{IOT_HUB_NAME}.azure-devices.net";

            var username = $"{IOT_HUB_NAME}.azure-devices.net/{IOT_HUB_DEVICE_ID}/api-version=2021-04-12";

            Resolver.Log.Info("Creating MQTT options ...");
            var options = new MqttClientOptionsBuilder()
                .WithClientId(IOT_HUB_DEVICE_ID)
                .WithTcpServer(iotHubUri, 8883)
                .WithCredentials(username, IOT_HUB_SAS_TOKEN)
                .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V311)
                .WithTls(new MqttClientOptionsBuilderTlsParameters
                {
                    UseTls = true,
                    SslProtocol = SslProtocols.Tls12,
                })
                .Build();


            Resolver.Log.Info("Connecting...");
            await mqttClient.ConnectAsync(options, new System.Threading.CancellationToken());

            isAuthenticated = true;
            return true;
        }
        catch (Exception ex)
        {
            Resolver.Log.Info($"{ex.Message}");
            isAuthenticated = false;
            return false;
        }
    }

    public async Task SendEnvironmentalReading((Temperature? Temperature, RelativeHumidity? Humidity) reading)
    {
        try
        {
            Resolver.Log.Info("Create payload");

            string messagePayload = $"" +
                $"{{" +
                $"\"temperature\":{reading.Temperature.Value.Celsius}," +
                $"\"humidity\":{reading.Humidity.Value.Percent}" +
                $"}}";

            Resolver.Log.Info("Create message");
            var mqttMessage = new MqttApplicationMessageBuilder()
                .WithTopic($"devices/{IOT_HUB_DEVICE_ID}/messages/events/")
                .WithPayload(messagePayload)
                .Build();

            await mqttClient.PublishAsync(mqttMessage, new System.Threading.CancellationToken());

            Resolver.Log.Info($"*** MQTT - DATA SENT - Temperature - {reading.Temperature.Value.Celsius}, Humidity - {reading.Humidity.Value.Percent} ***");
        }
        catch (Exception ex)
        {
            Resolver.Log.Info($"-- D2C Error - {ex.Message} --");
        }
    }
}