namespace MobileMaple.View
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
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