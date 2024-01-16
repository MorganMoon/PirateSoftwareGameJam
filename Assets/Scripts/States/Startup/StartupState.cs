using Cerberus;

namespace PirateSoftwareGameJam.Client.States.Startup
{
    public enum StartupStateEvents
    {
        PlayGame
    }

    public enum StartupStateSubStates
    {
        MainMenu,
        Credits
    }

    public class StartupState : State
    {
    }
}
