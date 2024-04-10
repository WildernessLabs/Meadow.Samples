namespace MobileBle.View
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            PermissionStatus location = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            PermissionStatus mearby = await Permissions.RequestAsync<Permissions.Bluetooth>();
        }

        void BtnLedClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LedControllerPage());
        }

        void BtnServoClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ServoControllerPage());
        }

        void BtnTemperatureClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TemperatureControllerPage());
        }
    }
}