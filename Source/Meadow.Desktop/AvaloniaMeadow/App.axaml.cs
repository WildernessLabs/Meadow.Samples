using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaMeadow.ViewModels;
using AvaloniaMeadow.Views;
using Meadow;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Atmospheric;
using Meadow.Peripherals.Leds;
using Meadow.UI;
using System.Threading.Tasks;

namespace AvaloniaMeadow
{
    public partial class App : AvaloniaMeadowApplication<Windows>
    {
        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);

            LoadMeadowOS();
        }

        public override Task MeadowInitialize()
        {
            var expander = FtdiExpanderCollection.Devices[0];

            var bme680 = new Bme680(expander.CreateSpiBus(), expander.Pins.C7);
            Resolver.Services.Add(bme680);

            var led = new Led(expander.Pins.C0);
            Resolver.Services.Add<ILed>(led);

            return Task.CompletedTask;
        }
    }
}