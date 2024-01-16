using Cerberus;
using UnityEngine;

namespace PirateSoftwareGameJam.Client.States.Startup.MainMenu
{
    public enum MainMenuStateEvents
    {
        ViewCredits
    }

    public class MainMenuState : State
    {
        public override void OnEnter()
        {
            Debug.Log("In the main menu!");
        }
    }
}
