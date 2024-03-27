using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Collections;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;

//https://docs.microsoft.com/en-us/azure/cognitive-services/Custom-Vision-Service/use-prediction-api
namespace BostonDetector
{
    // Change F7MicroV2 to F7Micro for V1.x boards
    public class MeadowApp : App<F7FeatherV2>
    {
        string endpoint = ""; //typically "https://[yourproject]-prediction.cognitiveservices.azure.com/";
        string projectId = ""; //from customvision.ai portal project url 
        string iterationId = "Iteration1"; //change as needed

        string url => $"{endpoint}/customvision/v3.0/Prediction/{projectId}/classify/iterations/{iterationId}/image";

        public override Task Initialize()
        {
            return InitNetwork();
        }

        async Task InitNetwork()
        {
            Console.WriteLine($"Connecting to WiFi Network {Secrets.WIFI_NAME}");

            var wiFiAdapter = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

            wiFiAdapter.NetworkConnected += (s, e) =>
            {
                Console.WriteLine($"IP Address: {wiFiAdapter.IpAddress}");
                Console.WriteLine($"Subnet mask: {wiFiAdapter.SubnetMask}");
                Console.WriteLine($"Gateway: {wiFiAdapter.Gateway}");
            };

            await wiFiAdapter.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD);
        }

        public override async Task Run()
        {
            string response = await PostImage("60-pup.jpg");
            var probability = GetAccuracyFromResponse(response);

            var msg2 = $"{probability * 100:0}% probability";

            Console.Write($"Boston detected: {msg2}");
        }

        async Task<string> PostImage(string imageFileName)
        {
            var startTime = DateTime.Now;

            byte[] byteData = LoadResource(imageFileName);

            Console.WriteLine($"LoadResource took: {(DateTime.Now - startTime).TotalSeconds}");

            using var content = new ByteArrayContent(byteData);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Prediction-Key", Secrets.PREDICTION_KEY);

            var response = await client.PostAsync(url, content);

            Console.WriteLine($"PostImage took: {(DateTime.Now - startTime).TotalSeconds}");

            return await response.Content.ReadAsStringAsync();
        }

        double GetAccuracyFromResponse(string response)
        {
            var resp = SimpleJsonSerializer.JsonSerializer.DeserializeString(response) as Hashtable;

            var items = (ArrayList)resp["predictions"];

            var bostonPrediction = (double)(items[0] as Hashtable)["probability"];

            return bostonPrediction;
        }

        byte[] LoadResource(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"BostonDetector.{filename}";

            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            using var ms = new MemoryStream();

            stream.CopyTo(ms);
            return ms.ToArray();
        }
    }
}