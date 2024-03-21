using Meadow;
using Meadow.Devices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Json_Basics
{
    public class Account
    {
        public string Email { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public IList<string> Roles { get; set; }
    }

    public class MeadowApp : App<F7FeatherV2>
    {
        public override Task Run()
        {
            Resolver.Log.Info("Hello Meadow Json serialize");

            TestJsonSerialize();

            TestJsonDeserialize();

            return Task.CompletedTask;
        }

        void TestJsonSerialize()
        {
            var account = new Account
            {
                Email = "james@example.com",
                Active = true,
                CreatedDate = new DateTime(2013, 1, 20, 0, 0, 0, DateTimeKind.Utc),
                Roles = new List<string>
                {
                    "User",
                    "Admin"
                }
            };

            string json = System.Text.Json.JsonSerializer.Serialize(account);

            Resolver.Log.Info($"Serialize:\r\n    {json}");
        }

        void TestJsonDeserialize()
        {
            string json = @"{
              ""Email"": ""james@example.com"",
              ""Active"": true,
              ""CreatedDate"": ""2013-01-20T00:00:00Z"",
              ""Roles"": [
                ""User"",
                ""Admin""
              ]
            }";

            Account account = System.Text.Json.JsonSerializer.Deserialize<Account>(json);

            Resolver.Log.Info($"Deserialize:\r\n" +
                $"    Email: {account.Email}\r\n" +
                $"    Active: {account.Active}\r\n" +
                $"    Created date: {account.CreatedDate}\r\n" +
                $"    Roles[0]: {account.Roles[0]}\r\n" +
                $"    Roles[1]: {account.Roles[1]}");
        }
    }
}