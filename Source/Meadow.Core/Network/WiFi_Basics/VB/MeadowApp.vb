Imports System.Net.Http
Imports System.Net.NetworkInformation
Imports Meadow
Imports Meadow.Devices
Imports Meadow.Devices.Esp32.MessagePayloads
Imports Meadow.Hardware

Public Class MeadowApp
    'Change F7FeatherV2 to F7FeatherV1 for V1.x boards'
    Inherits App(Of F7FeatherV2)

    Private WithEvents wifi As IWiFiNetworkAdapter

    Private Const SSID = "[YOUR_SSID_HERE]"
    Private Const WIFI_PASSWORD = "[YOUR_PASSPHRASE_HERE]"

    Public Overrides Async Function Run() As Task
        Console.WriteLine("Run...")

        wifi = Device.NetworkAdapters.Primary(Of IWiFiNetworkAdapter)

        ' enumerate the public WiFi channels
        Await ScanForAccessPoints(wifi)

        Try
            Console.WriteLine($"Connecting to WiFi Network {WIFI_NAME}")

            Await wifi.Connect(WIFI_NAME, WIFI_PASSWORD, TimeSpan.FromSeconds(45))

        Catch ex As Exception
            Resolver.Log.Error($"Failed to Connect: {ex.Message}")
        End Try

        If wifi.IsConnected Then
            DisplayNetworkInformation()

            While True
                Await GetWebPageViaHttpClient("https://postman-echo.com/get?foo1=bar1&foo2=bar2")
            End While
        End If
    End Function

    Private Sub OnNetworkConected(sender As INetworkAdapter, e As NetworkConnectionEventArgs) _
        Handles wifi.NetworkConnected

        Resolver.Log.Info("Connection request completed.")
    End Sub

    Private Async Function ScanForAccessPoints(adapter As IWiFiNetworkAdapter) As Task
        Resolver.Log.Info("Getting list of access points.")
        Dim networks = Await wifi.Scan(TimeSpan.FromSeconds(60))

        If networks.Count > 0 Then
            Resolver.Log.Info("|-------------------------------------------------------------|---------|")
            Resolver.Log.Info("|         Network Name             | RSSI |       BSSID       | Channel |")
            Resolver.Log.Info("|-------------------------------------------------------------|---------|")

            For Each accessPoint In networks
                Resolver.Log.Info($"| {accessPoint.Ssid,-32} | {accessPoint.SignalDbStrength,4} | {accessPoint.Bssid,17} |   {accessPoint.ChannelCenterFrequency,3}   |")
            Next
        Else
            Resolver.Log.Info($"No access points detected.")
        End If
    End Function

    Private Sub DisplayNetworkInformation()
        Dim adapters = NetworkInterface.GetAllNetworkInterfaces()

        If adapters.Length = 0 Then
            Resolver.Log.Info("No adapters available.")
        Else
            For Each ni In adapters
                Dim properties = ni.GetIPProperties()
                Resolver.Log.Info(String.Empty)
                Resolver.Log.Info(ni.Description)
                Resolver.Log.Info(String.Empty.PadLeft(ni.Description.Length, "="))
                Resolver.Log.Info($"  Adapter name: {ni.Name}")
                Resolver.Log.Info($"  Interface type .......................... : {ni.NetworkInterfaceType}")
                Resolver.Log.Info($"  Physical Address ........................ : {ni.GetPhysicalAddress()}")
                Resolver.Log.Info($"  Operational status ...................... : {ni.OperationalStatus}")

                Dim versions = String.Empty

                If ni.Supports(NetworkInterfaceComponent.IPv4) Then
                    versions = "IPv4"
                End If

                If ni.Supports(NetworkInterfaceComponent.IPv6) Then
                    If versions.Length > 0 Then
                        versions += " "
                    End If
                    versions += "IPv6"
                End If

                Resolver.Log.Info($"  IP version .............................. : {versions}")

                If ni.Supports(NetworkInterfaceComponent.IPv4) Then
                    Dim ipv4 = properties.GetIPv4Properties()
                    Resolver.Log.Info($"  MTU ..................................... : {ipv4.Mtu}")
                End If

                If (ni.NetworkInterfaceType = NetworkInterfaceType.Wireless80211) Or (ni.NetworkInterfaceType = NetworkInterfaceType.Ethernet) Then
                    For Each ip In ni.GetIPProperties().UnicastAddresses
                        If (ip.Address.AddressFamily = System.Net.Sockets.AddressFamily.InterNetwork) Then
                            Resolver.Log.Info($"  IP address .............................. : {ip.Address} [{wifi.IpAddress}]")
                            Resolver.Log.Info($"  Subnet mask ............................. : {ip.IPv4Mask}")
                        End If
                    Next
                End If
            Next
        End If
    End Sub

    Private Async Function GetWebPageViaHttpClient(uri As String) As Task
        Resolver.Log.Info($"Requesting {uri} - {Date.Now}")

        Using client = New HttpClient()
            client.Timeout = New TimeSpan(0, 5, 0)

            Dim response = Await client.GetAsync(uri)

            Try
                response.EnsureSuccessStatusCode()
                Dim responseBody = Await response.Content.ReadAsStringAsync()
                Resolver.Log.Info(responseBody)
            Catch tce As TaskCanceledException
                Resolver.Log.Info("Request time out.")
            Catch e As Exception
                Resolver.Log.Info($"Request went sideways: {e.Message}")
            End Try
        End Using
    End Function
End Class