using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;

namespace TLS_Client_Authentication;

public class MeadowApp : App<F7CoreComputeV2>
{
    public override Task Initialize()
    {
        var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();
        wifi.NetworkConnected += WiFiAdapter_NetworkConnected;

        return Task.CompletedTask;
    }

    async void WiFiAdapter_NetworkConnected(INetworkAdapter networkAdapter, NetworkConnectionEventArgs e)
    {
        // IMPORTANT: Steps before using this sample app
        //  1. Configure your Device on Azure IoT Hub
        //  2. Upload the client_cert.pem, private_key.pem and 
        //   private_key_pass.txt (optional) into your device
        //  3. Replace IOT_HUB_NAME and IOT_HUB_DEVICE_ID with your Azure IoT Hub data
        var client = new MQTTnetTestClient();
        await client.Initialize();
    }
}

class MQTTnetTestClient
{
    public async System.Threading.Tasks.Task Initialize()
    {
        // IMPORTANT:
        //  Replace these variables with your Azure IoT Hub data
        string IOT_HUB_NAME = "IOT_HUB_NAME";
        string IOT_HUB_DEVICE_ID = "IOT_HUB_DEVICE_ID";

        // IMPORTANT:
        //  Using Client Authentication, keep this value empty
        //  Using Token-based Authentication, replace it with your SAS token
        string IOT_HUB_DEVICE_ID_TOKEN = "";

        Console.WriteLine("Attempting to connect to " + IOT_HUB_DEVICE_ID);
        var factory = new MqttFactory();
        var mqttClient = factory.CreateMqttClient();

        // IMPORTANT:
        //  You don't need to add any code references to the client 
        //  certificate, since OS/Mono will attach your certificate 
        //  to any connection that requires it automatically
        var options = new MqttClientOptionsBuilder()
            .WithClientId(IOT_HUB_DEVICE_ID)
            .WithTcpServer($"{IOT_HUB_NAME}.azure-devices.net", 8883)
            .WithCredentials($"{IOT_HUB_NAME}.azure-devices.net/{IOT_HUB_DEVICE_ID}/?api-version=2021-04-12", IOT_HUB_DEVICE_ID_TOKEN)
            .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V311)
            .WithTls(new MqttClientOptionsBuilderTlsParameters
            {
                UseTls = true,
                SslProtocol = SslProtocols.Tls12,
            })
            .Build();

        mqttClient.UseConnectedHandler(async e =>
        {
            Console.WriteLine("Connected to broker");
        });

        var connected = false;
        while (!connected)
        {
            try
            {
                await mqttClient.ConnectAsync(options, CancellationToken.None);
                connected = true;
                Console.WriteLine("Connected!!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error! Please check your client certificate and private key files. ", ex);
            }
        }

        string[] messages = { "Accio!", "Aguamenti!", "Alarte Ascendare!", "Expecto Patronum!", "Homenum Revelio!" };
        foreach (var message in messages)
        {
            Console.WriteLine($"Sending message: {message}");
            var mqttMessage = new MqttApplicationMessageBuilder()
                .WithTopic($"devices/{IOT_HUB_DEVICE_ID}/messages/events/")
                .WithPayload(message)
                .Build();

            await mqttClient.PublishAsync(mqttMessage);
            await System.Threading.Tasks.Task.Delay(1000);
        }

        await mqttClient.DisconnectAsync();
    }
}