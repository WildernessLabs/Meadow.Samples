using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Displays.UI;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace MeadowMenu
{
    // public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
    public class MeadowApp : App<F7FeatherV2>
    {
        TextDisplayMenu menu;
        MicroGraphics graphics;
        PushButton next, previous, select;

        public override Task Initialize()
        {
            var onboardLed = new RgbPwmLed(
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            var st7789 = new St7789
            (
                spiBus: Device.CreateSpiBus(),
                chipSelectPin: null,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00,
                width: 240, height: 240
            );

            graphics = new MicroGraphics(st7789)
            {
                CurrentFont = new Font12x20(),
            };
            graphics.Clear();

            var menuData = LoadFromJson("menu.json");
            //var menuData = LoadFromCode();
            menu = new TextDisplayMenu(graphics, menuData, false);

            next = new PushButton(Device.Pins.D03, ResistorMode.InternalPullUp);
            next.Clicked += (s, e) => { menu.Next(); };

            select = new PushButton(Device.Pins.D04, ResistorMode.InternalPullUp);
            select.Clicked += (s, e) => { menu.Select(); };

            previous = new PushButton(Device.Pins.D01, ResistorMode.InternalPullUp);
            previous.Clicked += (s, e) => { menu.Previous(); };

            menu.Enable();

            onboardLed.SetColor(Color.Green);

            return base.Initialize();
        }

        byte[] LoadFromJson(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"MeadowMenu.{filename}";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }

        MenuItem[] LoadFromCode()
        {
            var subMenuItems2 = new List<MenuItem>();
            subMenuItems2.Add(new MenuItem(
                text: "Sub Item C"));
            subMenuItems2.Add(new MenuItem(
                text: "Sub Item D"));
            subMenuItems2.Add(new MenuItem(
                text: "Sub Item E"));

            var subMenuItems1 = new List<MenuItem>();
            subMenuItems1.Add(new MenuItem(
                text: "Sub Item A"));
            subMenuItems1.Add(new MenuItem(
                text: "Sub Item B"));
            subMenuItems1.Add(new MenuItem(
                text: "Submenu 2",
                subItems: subMenuItems2.ToArray()));

            var menuItems = new List<MenuItem>();
            menuItems.Add(new MenuItem(
                text: "Value 1: {value}",
                id: "displayValue1",
                value: 77));
            menuItems.Add(new MenuItem(
                text: "Edit Value 1",
                id: "myValue1",
                type: "Value",
                value: 77));
            menuItems.Add(new MenuItem(
                text: "Value 2: {value}",
                id: "displayValue1",
                value: 25));
            menuItems.Add(new MenuItem(
                text: "Edit Value 2",
                id: "myValue2",
                type: "Value",
                value: 25));
            menuItems.Add(new MenuItem(
                text: "submenu 1",
                subItems: subMenuItems1.ToArray()));

            return menuItems.ToArray();
        }
    }
}