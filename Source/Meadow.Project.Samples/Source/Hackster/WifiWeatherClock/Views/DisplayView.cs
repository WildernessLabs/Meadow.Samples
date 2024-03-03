using Meadow.Foundation.Displays.Lcd;

namespace WifiWeatherClock.Views
{
    public class DisplayView
    {
        CharacterDisplay display;
        
        public DisplayView()
        {
            Initialize();

            ShowSplashScreen();
        }

        void Initialize()
        {
            display = new CharacterDisplay
            (
                pinRS:  MeadowApp.Device.Pins.D10,
                pinE:   MeadowApp.Device.Pins.D09,
                pinD4:  MeadowApp.Device.Pins.D08,
                pinD5:  MeadowApp.Device.Pins.D07,
                pinD6:  MeadowApp.Device.Pins.D06,
                pinD7:  MeadowApp.Device.Pins.D05,
                rows: 4, columns: 20
            );
        }

        void ShowSplashScreen() 
        {
            display.WriteLine($"--------------------", 0);
            display.WriteLine($" WIFI Weather Clock ", 1);
            display.WriteLine($"     Loading...     ", 2);
            display.WriteLine($"--------------------", 3);
        }

        public void WriteLine(string text, byte lineNumber) 
        {
            display.WriteLine($"{text}", lineNumber);
        }
    }
}