using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpListener_Basics;

public class MeadowApp : App<F7FeatherV2>
{
    public static HttpListener listener;
    public IPAddress ipAddress;
    public int port = 8081;
    public static int pageViews = 0;
    public static int requestCount = 0;
    public static string pageData =
        "<!DOCTYPE>" +
        "<html>" +
        "  <head>" +
        "    <title>HttpListener Example</title>" +
        "  </head>" +
        "  <body>" +
        "    <p>Page Views: {0}</p>" +
        "    <form method=\"post\" action=\"shutdown\">" +
        "      <input type=\"submit\" value=\"Shutdown\" {1}>" +
        "    </form>" +
        "  </body>" +
        "</html>";

    protected string Url
    {
        get
        {
            if (ipAddress != null)
            {
                return $"http://{ipAddress}:{port}/";
            }
            else
            {
                return $"http://127.0.0.1:{port}/";
            }
        }
    }

    /// <summary>
    /// Default constructor.
    /// </summary>
    public override Task Initialize()
    {
        Resolver.Log.Info("Creating HttpListenerTest object.");

        var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

        Resolver.Log.Info("Connecting to access point.");
        wifi.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD).Wait();

        Resolver.Log.Info("WiFi connection completed.");
        ipAddress = wifi.IpAddress;

        return Task.CompletedTask;
    }

    public override Task Run()
    {
        StartServer();

        return Task.CompletedTask;
    }

    void WiFiAdapter_WiFiConnected(object sender, EventArgs e)
    {
        Resolver.Log.Info("WiFiAdapter_WiFiConnected: connected to access point.");
    }

    public async Task HandleIncomingConnections()
    {
        bool runServer = true;

        await Task.Run(async () =>
        {
            // While a user hasn't visited the `shutdown` url, keep on handling requests
            while (runServer)
            {
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = await listener.GetContextAsync();

                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                // Print out some info about the request
                Resolver.Log.Info($"Request #: {++requestCount}");
                Resolver.Log.Info(req.Url.ToString());
                Resolver.Log.Info(req.HttpMethod);
                Resolver.Log.Info(req.UserHostName);
                Resolver.Log.Info(req.UserAgent);

                // If `shutdown` url requested w/ POST, then shutdown the server after serving the page
                if ((req.HttpMethod == "POST") && (req.Url.AbsolutePath == "/shutdown"))
                {
                    Resolver.Log.Info("Shutdown requested");
                    runServer = false;
                }

                // Make sure we don't increment the page views counter if `favicon.ico` is requested
                if (req.Url.AbsolutePath != "/favicon.ico")
                {
                    pageViews += 1;
                }

                // Write the response info
                string disableSubmit = !runServer ? "disabled" : "";
                byte[] data = Encoding.UTF8.GetBytes(String.Format(pageData, pageViews, disableSubmit));
                resp.ContentType = "text/html";
                resp.ContentEncoding = Encoding.UTF8;
                resp.ContentLength64 = data.LongLength;

                // Write out to the response stream (asynchronously), then close it
                await resp.OutputStream.WriteAsync(data, 0, data.Length);
                resp.Close();
            }
        });
    }

    public async void StartServer()
    {
        listener = new HttpListener();
        listener.Prefixes.Add(Url);
        listener.Start();
        Resolver.Log.Info($"Listening for connections on {Url}");

        // Handle requests
        await HandleIncomingConnections();

        // Close the listener
        listener.Close();
    }
}