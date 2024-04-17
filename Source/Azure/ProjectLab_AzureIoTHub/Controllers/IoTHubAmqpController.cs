using Amqp;
using Amqp.Framing;
using Meadow;
using Meadow.Units;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLab_AzureIoTHub.Controllers;

/// <summary>
/// You'll need to create an IoT Hub - https://learn.microsoft.com/en-us/azure/iot-hub/iot-hub-create-through-portal
/// Create a device within your IoT Hub
/// And then generate a SAS token - this can be done via the Azure CLI 
/// </summary>
internal class IoTHubAmqpController : IIoTHubController
{
    private const string HubName = Secrets.HUB_NAME;
    private const string SasToken = Secrets.SAS_TOKEN;
    private const string DeviceId = Secrets.DEVICE_ID;

    private Connection connection;
    private SenderLink sender;

    public bool isAuthenticated { get; private set; }

    public IoTHubAmqpController() { }

    public async Task<bool> Initialize()
    {
        try
        {
            string hostName = HubName + ".azure-devices.net";
            string userName = DeviceId + "@sas." + HubName;
            string senderAddress = "devices/" + DeviceId + "/messages/events";

            Resolver.Log.Info("Create connection factory...");
            var factory = new ConnectionFactory();

            Resolver.Log.Info("Create connection ...");
            connection = await factory.CreateAsync(new Address(hostName, 5671, userName, SasToken));

            Resolver.Log.Info("Create session ...");
            var session = new Session(connection);

            Resolver.Log.Info("Create SenderLink ...");
            sender = new SenderLink(session, "send-link", senderAddress);

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

    public Task SendEnvironmentalReading((Temperature? Temperature, RelativeHumidity? Humidity, Pressure? Pressure, Resistance? GasResistance) reading)
    {
        try
        {
            Resolver.Log.Info("Create payload");

            string messagePayload = $"" +
                    $"{{" +
                    $"\"temperature\":{reading.Temperature.Value.Celsius}," +
                    $"\"humidity\":{reading.Humidity.Value.Percent}," +
                    $"\"pressure\":{reading.Pressure.Value.Millibar}" +
                    $"}}";

            Resolver.Log.Info("Create message");
            var payloadBytes = Encoding.UTF8.GetBytes(messagePayload);
            var message = new Message()
            {
                BodySection = new Data() { Binary = payloadBytes }
            };

            sender.SendAsync(message);

            Resolver.Log.Info($"*** AMQP - DATA SENT - Temperature - {reading.Temperature.Value.Celsius}, Humidity - {reading.Humidity.Value.Percent}, Pressure - {reading.Pressure.Value.Millibar} ***");
        }
        catch (Exception ex)
        {
            Resolver.Log.Info($"-- D2C Error - {ex.Message} --");
        }

        return Task.CompletedTask;
    }
}