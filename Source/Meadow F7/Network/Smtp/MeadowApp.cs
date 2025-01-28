using Meadow;
using Meadow.Devices;
using Meadow.Gateway.WiFi;
using Meadow.Hardware;
using System.Net.NetworkInformation;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace Smtp;

public class MeadowApp : App<F7FeatherV2>
{
    private const string WIFI_NAME = "WIFI_NAME";
    private const string WIFI_PASSWORD = "WIFI_PASSWORD";
    private IWiFiNetworkAdapter wifi;

    public override async Task Initialize()
    {
        Resolver.Log.Info("Initialize...");
        wifi = Resolver.Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

        try
        {
            await wifi.Connect(WIFI_NAME, WIFI_PASSWORD, TimeSpan.FromSeconds(45));
        }
        catch (Exception ex)
        {
            Resolver.Log.Error($"Failed to Connect: {ex.Message}");
        }
    }

    public override Task Run()
    {
        string fromMail = "YOUR_EMAIL@mail.com";
        string toMail = "YOUR_EMAIL@mail.com";
        string passwordMail = "YOUR_PASSOWOR_APPLICATION";

        MailMessage message = new MailMessage();
        message.From = new MailAddress(fromMail);
        message.To.Add(new MailAddress(toMail));
        message.Subject = "Meadow SMTP";
        message.Body = "This message was sent from Meadow F7";
        message.IsBodyHtml = false;

        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(fromMail, passwordMail),
            EnableSsl = true,
        };
        Resolver.Log.Info("Sending the Email ...");
        smtpClient.Send(message);

        return Task.CompletedTask;
    }
}