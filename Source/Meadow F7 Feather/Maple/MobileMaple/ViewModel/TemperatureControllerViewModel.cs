
using MobileMaple.Model;
using System.Collections.ObjectModel;

namespace MobileMaple.ViewModel
{
    public class TemperatureControllerViewModel : BaseViewModel
    {
        public ObservableCollection<TemperatureModel> TemperatureLog { get; set; }

        public TemperatureControllerViewModel() 
        {
            TemperatureLog = new ObservableCollection<TemperatureModel>();

            SendCommand = new Command(async (obj) => await GetTemperatureLogs());
        }

        async Task GetTemperatureLogs()
        {
            if(IsBusy) 
                return;
            IsBusy= true;

            try
            {
                var response = await client.GetAsync(
                    hostAddress: SelectedServer != null ? SelectedServer.IpAddress : IpAddress,
                    port: ServerPort,
                    endPoint: "gettemperaturelogs");

                if (string.IsNullOrEmpty(response))
                    return;

                var values = System.Text.Json.JsonSerializer.Deserialize<List<TemperatureModel>>(response);

                TemperatureLog.Clear();
                foreach (var value in values)
                {
                    TemperatureLog.Add(value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            { 
                IsBusy = false; 
            }
        }

    }
}