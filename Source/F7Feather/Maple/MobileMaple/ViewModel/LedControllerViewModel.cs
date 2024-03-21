namespace MobileMaple.ViewModel
{
    public class LedControllerViewModel : BaseViewModel
    {
        bool isOn;
        public bool IsOn
        {
            get => isOn;
            set { isOn = value; OnPropertyChanged(nameof(IsOn)); }
        }

        bool isOff;
        public bool IsOff
        {
            get => isOff;
            set { isOff = value; OnPropertyChanged(nameof(IsOff)); }
        }

        bool isPulsing;
        public bool IsPulsing
        {
            get => isPulsing;
            set { isPulsing = value; OnPropertyChanged(nameof(IsPulsing)); }
        }

        bool isBlinking;
        public bool IsBlinking
        {
            get => isBlinking;
            set { isBlinking = value; OnPropertyChanged(nameof(IsBlinking)); }
        }

        bool isRunningColors;
        public bool IsRunningColors
        {
            get => isRunningColors;
            set { isRunningColors = value; OnPropertyChanged(nameof(IsRunningColors)); }
        }

        public LedControllerViewModel() : base()
        {
            SendCommand = new Command(async (obj) => await SendLedCommand(obj as string));

            IsOn = true;
        }

        async Task SendLedCommand(string command)
        {
            if (IsBusy || string.IsNullOrEmpty(IpAddress))
                return;
            IsBusy = true;

            try
            {
                bool response = await client.PostAsync(SelectedServer != null ? SelectedServer.IpAddress : IpAddress, ServerPort, command, string.Empty);

                if (response)
                {
                    IsOn = IsOff = IsBlinking = IsPulsing = IsRunningColors = false;

                    switch (command)
                    {
                        case "TurnOn": IsOn = true; break;
                        case "TurnOff": IsOff = true; break;
                        case "StartBlink": IsBlinking = true; break;
                        case "StartPulse": IsPulsing = true; break;
                        case "StartRunningColors": IsRunningColors = true; break;
                    }
                }
                else
                {
                    Console.WriteLine("Request failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}