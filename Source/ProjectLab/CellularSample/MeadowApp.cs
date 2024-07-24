using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Hardware;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace CellularSample;

public class MeadowApp : ProjectLabCoreComputeApp
{
    private DisplayController? displayController;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        var cell = Hardware.ComputeModule.NetworkAdapters.Primary<ICellNetworkAdapter>();
        cell.NetworkDisconnected += CellAdapter_NetworkDisconnected;
        cell.NetworkConnected += CellAdapter_NetworkConnected;

        Resolver.Log.Info($"Running on ProjectLab Hardware {Hardware.RevisionString}");

        if (Hardware.RgbLed is { } rgbLed)
        {
            rgbLed.SetColor(Color.Blue);
        }

        if (Hardware.Display is { } display)
        {
            displayController = new DisplayController(display);

            displayController.UpdateSignalBar(cell.GetSignalQuality());
            displayController.UpdateStatus(cell.IsConnected ? "CONNECTED" : "DISCONNECTED");
            displayController.UpdateIpAddress(cell.IsConnected ? cell.IpAddress.ToString() : "---.---.---.---");
        }

        return Task.CompletedTask;
    }

    void CellAdapter_NetworkDisconnected(INetworkAdapter sender, NetworkDisconnectionEventArgs args)
    {
        displayController.UpdateStatus("DISCONNECTED");
        displayController.UpdateIpAddress("---.---.---.---");
        displayController.UpdateSignalBar(-9999);
    }

    async void CellAdapter_NetworkConnected(INetworkAdapter networkAdapter, NetworkConnectionEventArgs e)
    {
        var cell = networkAdapter as ICellNetworkAdapter;

        if (cell != null)
        {
            Resolver.Log.Info("Cell CSQ at the time of connection (dbm): " + cell.Csq);
            Resolver.Log.Info("Cell IMEI: " + cell.Imei);

            displayController.UpdateStatus("CONNECTED");
            displayController.UpdateIpAddress(cell.IpAddress.ToString());
            displayController.UpdateSignalBar(cell.Csq);

            await GetWebPageViaHttpClient("https://postman-echo.com/get?fool=bar1&foo2=bar2");
        }
    }

    async Task GetWebPageViaHttpClient(string uri)
    {
        Resolver.Log.Info($"Requesting {uri} - {DateTime.Now}");
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        using (HttpClient client = new HttpClient())
        {
            // In weak signal connections and/or large download scenarios, it's recommended to increase the client timeout
            client.Timeout = TimeSpan.FromMinutes(5);
            using (HttpResponseMessage response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead))
            {
                try
                {
                    response.EnsureSuccessStatusCode();

                    var contentLength = response.Content.Headers.ContentLength ?? -1L;
                    var progress = new Progress<long>(totalBytes =>
                    {
                        Resolver.Log.Info($"{totalBytes} bytes downloaded ({(double)totalBytes / contentLength:P2})");
                    });

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        var buffer = new byte[4096];
                        long totalBytesRead = 0;
                        int bytesRead;

                        while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            totalBytesRead += bytesRead;
                            ((IProgress<long>)progress).Report(totalBytesRead);
                        }
                    }

                    stopwatch.Stop();
                    Resolver.Log.Info($"Download complete. Time taken: {stopwatch.Elapsed.TotalSeconds:F2} seconds");
                }
                catch (TaskCanceledException)
                {
                    Resolver.Log.Info("Request timed out.");
                }
                catch (Exception e)
                {
                    Resolver.Log.Info($"Request went sideways: {e.Message}");
                }
            }
        }
    }

    public override Task Run()
    {
        Resolver.Log.Info("Run...");

        if (Hardware?.RgbLed is { } rgbLed)
        {
            Resolver.Log.Info("starting blink");
            _ = rgbLed.StartBlink(WildernessLabsColors.PearGreen, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(2000), 0.5f);
        }

        return Task.CompletedTask;
    }
}