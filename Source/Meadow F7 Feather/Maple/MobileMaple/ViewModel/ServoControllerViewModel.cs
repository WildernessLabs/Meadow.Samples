namespace MobileMaple.ViewModel
{
    public class ServoControllerViewModel : BaseViewModel
    {
        int angleDegrees;
        public int AngleDegrees
        {
            get => angleDegrees;
            set { angleDegrees = value; OnPropertyChanged(nameof(AngleDegrees)); }
        }

        bool _isRotateTo;
        public bool IsRotateTo
        {
            get => _isRotateTo;
            set { _isRotateTo = value; OnPropertyChanged(nameof(IsRotateTo)); }
        }

        bool _isCyclingStart;
        public bool IsCyclingStart
        {
            get => _isCyclingStart;
            set { _isCyclingStart = value; OnPropertyChanged(nameof(IsCyclingStart)); }
        }

        bool _isCyclingStop;
        public bool IsCyclingStop
        {
            get => _isCyclingStop;
            set { _isCyclingStop = value; OnPropertyChanged(nameof(IsCyclingStop)); }
        }

        public ServoControllerViewModel() : base()
        {
            SendCommand = new Command(async (obj) => await SendServoCommand(obj as string));

            IsCyclingStop = true;
        }

        async Task SendServoCommand(string command)
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                bool response = await client.PostAsync(SelectedServer != null ? SelectedServer.IpAddress : IpAddress, ServerPort, command, AngleDegrees.ToString());

                if (response)
                {
                    IsCyclingStart = IsCyclingStop = IsRotateTo = false;

                    switch (command)
                    {
                        case "RotateTo": IsRotateTo = true; break;
                        case "StartSweep": IsCyclingStart = true; break;
                        case "StopSweep": IsCyclingStop = true; break;
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