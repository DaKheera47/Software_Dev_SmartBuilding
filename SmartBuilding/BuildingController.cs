namespace SmartBuilding
{
    public class BuildingController
    {
        // Write BuildingController code here...
        // define building variables
        private string? buildingID;
        private string currentState;
        public string? historyState;
        readonly private string[] regularStates = { "closed", "out of hours", "open" };
        readonly private string[] emergencyStates = { "fire drill", "fire alarm" };
        private string[] allValidStates;

        // managers
        private readonly ILightManager? iLightManager;
        private readonly IFireAlarmManager? iFireAlarmManager;
        private readonly IDoorManager? iDoorManager;
        private readonly IWebService? iWebService;
        private readonly IEmailService? iEmailService;

        // constructor
        public BuildingController(string id)
        {
            // set building id
            SetBuildingID(id);
            this.currentState = "out of hours";
            this.historyState = this.currentState;
            this.allValidStates = regularStates.Concat(emergencyStates).ToArray();
        }

        // additional constructor with buildingID and currentState
        public BuildingController(string id, string startState)
        {
            startState = startState.ToLower();

            this.allValidStates = regularStates.Concat(emergencyStates).ToArray();

            // set current state
            if (!regularStates.Contains(startState))
            {
                throw new System.ArgumentException("Argument Exception: BuildingController can only be initialised to the following states 'open', 'closed', 'out of hours'");
            }

            // set building id
            SetBuildingID(id);

            // set SetCurrentState
            SetCurrentState(startState);
        }

        // L3R1
        public BuildingController(string id, ILightManager iLightManager, IFireAlarmManager iFireAlarmManager, IDoorManager iDoorManager, IWebService iWebService, IEmailService iEmailService)
        {
            this.allValidStates = regularStates.Concat(emergencyStates).ToArray();
            this.iLightManager = iLightManager;
            this.iFireAlarmManager = iFireAlarmManager;
            this.iDoorManager = iDoorManager;
            this.iWebService = iWebService;
            this.iEmailService = iEmailService;

            // set building id
            SetBuildingID(id);

            // set current state
            SetCurrentState("out of hours");
        }

        // L3R3
        public string GetStatusReport()
        {
            if (iLightManager == null || iFireAlarmManager == null || iDoorManager == null)
            {
                throw new System.ArgumentException("Argument Exception: BuildingController has not been initialised with all the required managers");
            }

            // get status of all managers
            string lightStatus = iLightManager.GetStatus();
            string doorStatus = iDoorManager.GetStatus();
            string fireAlarmStatus = iFireAlarmManager.GetStatus();

            // combine all status into one string
            // in order of light, door, fire alarm
            string statusReport = lightStatus + doorStatus + fireAlarmStatus;

            return statusReport;
        }

        // set building id
        public void SetBuildingID(string id)
        {
            // convert to lowercase
            id = id.ToLower();

            // convert building id to int
            this.buildingID = id;
        }

        // get building id
        public string? GetBuildingID()
        {
            return buildingID;
        }

        // get current state
        public string GetCurrentState()
        {
            return currentState;
        }

        // set current state
        public bool SetCurrentState(string state)
        {
            string incomingState = state;

            // if current state is null, set to incoming state
            if (currentState == null)
            {
                currentState = incomingState;
                return true;
            }

            // if current state is the same as incoming state
            if (incomingState == currentState) { return true; }

            // Local dictionary for state transitions
            /*  Valid transitions:
                closed -> out of hours
                out of hours -> closed, open
                open -> out of hours
            */
            var transitions = new Dictionary<string, string[]> {
                { "closed", new[] { "out of hours" } },
                { "out of hours", new[] { "closed", "open" } },
                { "open", new[] { "out of hours" } }
            };

            // Check if the current state allows transitioning to the new state
            if (transitions[currentState].Contains(incomingState))
            {
                // Update historyState before changing currentState
                historyState = currentState;
                currentState = incomingState;

                return true;
            }

            return false;



            // set state to lowercase
            // state = state.ToLower();

            // // if invalid state
            // if (!allValidStates.Contains(state)) { return false; }

            // // if state being set to is open, then open all doors
            // if (state == "open")
            // {
            //     if (iDoorManager != null)
            //     {
            //         bool openedDoors = iDoorManager.OpenAllDoors();

            //         // if doors did not open
            //         if (!openedDoors) { return false; }
            //     }
            // }

            // // Remember the previous state before changing to a new state
            // if (!emergencyStates.Contains(state))
            // {
            //     historyState = currentState;
            // }

            // // if current state is in emergency, then do not change the state unless it is reverting to history state
            // if (emergencyStates.Contains(currentState) && state != historyState)
            // {
            //     return false;
            // }

            // // if null, set to current state
            // currentState ??= state;

            // return true;
        }
    }
}