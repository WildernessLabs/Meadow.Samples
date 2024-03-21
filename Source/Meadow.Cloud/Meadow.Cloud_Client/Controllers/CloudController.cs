using Meadow.Cloud_Client.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Meadow.Cloud_Client.Controllers;

internal class CloudController
{
    public async Task<List<QueryResponse>> GetSensorReadings()
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Authorization", $"apikey {Secrets.API_KEY}");
                client.Timeout = new TimeSpan(0, 5, 0);

                Resolver.Log.Trace($"Request sent...");

                HttpResponseMessage response = await client.GetAsync($"{Secrets.MEADOW_CLOUD_URL}/api/orgs/{Secrets.ORGANIZATION_ID}/search/source:event deviceId:{Secrets.DEVICE_ID} eventId:1000 size:50 sortby:timestamp sortorder:desc");

                if (response.IsSuccessStatusCode)
                {
                    Resolver.Log.Trace($"Request complete!");

                    string jsonString = await response.Content.ReadAsStringAsync();
                    //Resolver.Log.Trace("Response: " + jsonString);

                    Resolver.Log.Trace($"Deserializing response...");
                    var root = JsonSerializer.Deserialize<Root>(jsonString);
                    Resolver.Log.Trace($"Deserializing complete!");

                    return root?.data?.queryResponses;
                }
                else
                {
                    Resolver.Log.Error("Error: " + response.StatusCode);
                    return new List<QueryResponse>();
                }
            }
            catch (TaskCanceledException)
            {
                Resolver.Log.Error("Request timed out.");
                return new List<QueryResponse>();
            }
            catch (Exception e)
            {
                Resolver.Log.Error($"Request went sideways: {e.Message}");
                return new List<QueryResponse>();
            }
        }
    }
}