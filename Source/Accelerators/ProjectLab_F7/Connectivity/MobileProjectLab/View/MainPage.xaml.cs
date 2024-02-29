namespace MobileProjectLab.View
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        void BtnMapleClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MaplePage());
        }

        void BtnBluetoothClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new BluetoothPage());
        }
    }
}