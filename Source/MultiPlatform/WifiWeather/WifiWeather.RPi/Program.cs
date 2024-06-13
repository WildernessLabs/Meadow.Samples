using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Logging;
using System;
using System.Threading.Tasks;
using wifiweather.Core;

namespace wifiweather.RPi
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            MeadowOS.Start(args);
        }
    }
}