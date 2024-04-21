using MobileGnssTrackerConnectivity.ViewModel;

namespace MobileGnssTrackerConnectivity.View
{
    public partial class MaplePage : ContentPage
    {
        public MaplePage()
        {
            InitializeComponent();
            BindingContext = new MapleViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as MapleViewModel).CmdSearchServers.Execute(null);
        }
    }
}