using Amqp;
using Meadow;
using MeadowAzureServo.Models;
using System;
using System.Threading.Tasks;

namespace MeadowAzureServo.Azure
{
    /// <summary>
    /// You'll need to create an IoT Hub - https://learn.microsoft.com/en-us/azure/iot-hub/iot-hub-create-through-portal
    /// Create a device within your IoT Hub
    /// And then generate a SAS token - this can be done via the Azure CLI 
    /// </summary>
    /*
     az iot hub generate-sas-token --hub-name HUB_NAME --device-id DEVICE_ID --resource-group RESOURCE_GROUP --login [Open Shared access policies -> Select iothubowner -> copy Primary connection string]
    */
    public class IoTHubManager
    {
        private const string HubName = Secrets.HUB_NAME;
        private const string SasToken = Secrets.SAS_TOKEN;
        private const string DeviceId = Secrets.DEVICE_ID;

        private Connection connection;
        private ReceiverLink receiver;

        public event EventHandler StartSweeping = delegate { };

        public event EventHandler StopSweeping = delegate { };

        public event EventHandler<RotateToEventArgs> RotateTo = delegate { };

        public IoTHubManager() { }

        public async Task Initialize()
        {
            string hostName = HubName + ".azure-devices.net";
            string userName = DeviceId + "@sas." + HubName;
            string receiverAddress = "devices/" + DeviceId + "/messages/deviceBound";

            Resolver.Log.Info("Create connection factory...");
            var factory = new ConnectionFactory();

            Resolver.Log.Info("Create connection ...");
            connection = await factory.CreateAsync(new Address(hostName, 5671, userName, SasToken));

            Resolver.Log.Info("Create session ...");
            var session = new Session(connection);

            Resolver.Log.Info("Create ReceiverLink ...");
            receiver = new ReceiverLink(session, "receive-link", receiverAddress);
            receiver.Start(100, OnMessage);

            Resolver.Log.Info("Ready to receive messages ...");
        }

        private void OnMessage(IReceiverLink receiver, Message message)
        {
            try
            {
                string command = (string)message.ApplicationProperties["command"];

                if (command == "StartSweeping")
                {
                    Resolver.Log.Info($"== Received StartSweeping Command ==");
                    StartSweeping?.Invoke(this, new EventArgs());

                }
                else if (command == "StopSweeping")
                {
                    Resolver.Log.Info($"== Received StopSweeping Command ==");
                    StopSweeping?.Invoke(this, new EventArgs());
                }
                else if (command == "RotateTo")
                {
                    Resolver.Log.Info($"== Received RotateTo Command ==");

                    int angle = 0;
                    int.TryParse((string)message.ApplicationProperties["angle"], out angle);
                    RotateTo?.Invoke(this, new RotateToEventArgs(angle));
                }
                else
                {
                    Resolver.Log.Error("Command not identified.");
                }

                receiver.Accept(message);
            }
            catch (Exception ex)
            {
                Resolver.Log.Info($"-- C2D Error - {ex.Message} --");
            }
        }
    }
}