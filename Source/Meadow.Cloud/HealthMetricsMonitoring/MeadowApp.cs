using Meadow;
using Meadow.Devices;
using Meadow.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HealthMetricsMonitoring
{
    // Change F7FeatherV2 to F7FeatherV1 for V1.x boards
    public class MeadowApp : App<F7FeatherV2>
    {
        public override Task Initialize()
        {
            var cloudLogger = new CloudLogger();
            Resolver.Log.AddProvider(cloudLogger);
            Resolver.Services.Add(cloudLogger);
            return base.Initialize();
        }

        public override async Task Run()
        {
            System.Timers.Timer timer = new(interval: 1 * 60 * 1000);
            timer.Elapsed += (_, _) =>
            {
                var cl = Resolver.Services.Get<CloudLogger>();
                try
                {
                    throw new Exception($"this is a test exception for logging {WordFinder2(10)}");
                }
                catch (Exception ex)
                {
                    cl.LogException(ex);
                }

                Resolver.Log.Info($"log loop {WordFinder2(10)}");
            };
            timer.AutoReset = true;
            timer.Start();

            await Task.Delay(Timeout.Infinite);
        }

        public static string WordFinder2(int requestedLength)
        {
            Random rnd = new Random();
            string[] consonants =
                { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "y", "z" };
            string[] vowels = { "a", "e", "i", "o", "u" };

            string word = "";

            if (requestedLength == 1)
            {
                word = GetRandomLetter(rnd, vowels);
            }
            else
            {
                for (int i = 0; i < requestedLength; i += 2)
                {
                    word += GetRandomLetter(rnd, consonants) + GetRandomLetter(rnd, vowels);
                }

                word = word.Replace("q", "qu")
                    .Substring(0,
                        requestedLength); // We may generate a string longer than requested length, but it doesn't matter if cut off the excess.
            }

            return word;
        }

        private static string GetRandomLetter(Random rnd, string[] letters)
        {
            return letters[rnd.Next(0, letters.Length - 1)];
        }
    }
}