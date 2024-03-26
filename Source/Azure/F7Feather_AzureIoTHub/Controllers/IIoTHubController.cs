using Meadow.Units;
using System.Threading.Tasks;

namespace MeadowAzureIoTHub.Controllers
{
    /// <summary>
    /// You'll need to create an IoT Hub - https://learn.microsoft.com/en-us/azure/iot-hub/iot-hub-create-through-portal
    /// Create a device within your IoT Hub
    /// And then generate a SAS token - this can be done via the Azure CLI 
    /// 
    /// Example
    /// az iot hub generate-sas-token
    /// --hub-name HUB_NAME 
    /// --device-id DEVICE_ID 
    /// --resource-group RESOURCE_GROUP 
    /// --login [Open Shared access policies -> Select iothubowner -> copy Primary connection string]
    /// </summary>
    internal interface IIoTHubController
    {
        bool isAuthenticated { get; }

        Task<bool> Initialize();

        Task SendEnvironmentalReading((Temperature? Temperature, RelativeHumidity? Humidity) reading);
    }
}