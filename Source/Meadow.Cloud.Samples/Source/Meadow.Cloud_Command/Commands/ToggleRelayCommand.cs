using Meadow.Cloud;

namespace Meadow.Cloud_Command.Commands
{
    /*

Command Name:

    ToggleRelayCommand

Arguments:

{
    "Relay" : 4,
    "IsOn": true
}

*/

    public class ToggleRelayCommand : IMeadowCommand
    {
        public int Relay { get; set; }

        public bool IsOn { get; set; }
    }
}