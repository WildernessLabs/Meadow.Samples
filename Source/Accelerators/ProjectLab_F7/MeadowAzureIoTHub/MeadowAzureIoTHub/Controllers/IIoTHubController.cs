using Meadow.Units;
using System.Threading.Tasks;

namespace MeadowAzureIoTHub.Controllers
{
    internal interface IIoTHubController
    {
        bool isAuthenticated { get; }

        Task<bool> Initialize();

        Task SendEnvironmentalReading((Temperature? Temperature, RelativeHumidity? Humidity, Pressure? Pressure, Resistance? GasResistance) reading);
    }
}