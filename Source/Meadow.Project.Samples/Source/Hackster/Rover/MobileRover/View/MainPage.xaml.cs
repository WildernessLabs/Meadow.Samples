using MobileRover.ViewModel;

namespace MobileRover.View
{
    public partial class MainPage : ContentPage
    {
        MainViewModel vm;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = vm = new MainViewModel();

            btnUp.Pressed += async (s, e) => { await vm.MoveForward(true); };
            btnUp.Released += async (s, e) => { await vm.MoveForward(false); };
            btnDown.Pressed += async (s, e) => { await vm.MoveBackward(true); };
            btnDown.Released += async (s, e) => { await vm.MoveBackward(false); };
            btnLeft.Pressed += async (s, e) => { await vm.TurnLeft(true); };
            btnLeft.Released += async (s, e) => { await vm.TurnLeft(false); };
            btnRight.Pressed += async (s, e) => { await vm.TurnRight(true); };
            btnRight.Released += async (s, e) => { await vm.TurnRight(false); };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.CmdSearchForDevices.Execute(null);
        }
    }
}