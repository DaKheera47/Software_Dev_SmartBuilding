namespace SmartBuilding
{
    public class BuildingController
    {
        // Write BuildingController code here...
        // define building variables
        private string? buildingId;
        private string? currentState;
        private string? previousState;
        readonly private string[] regularStates = { "closed", "out of hours", "open" };
        readonly private string[] emergencyStates = { "fire drill", "fire alarm" };
        private string[] allValidStates;

        // managers
        private ILightManager? iLightManager;
        private IFireAlarmManager? iFireAlarmManager;
        private IDoorManager? iDoorManager;
        private IWebService? iWebService;
        private IEmailService? iEmailService;

        // constructor
        public BuildingController(string buildingId)
        {
            // set building id
            SetBuildingId(buildingId);
            this.currentState = "out of hours";
            this.previousState = this.currentState;
            this.allValidStates = regularStates.Concat(emergencyStates).ToArray();
        }

        // additional constructor with buildingID and currentState
        public BuildingController(string buildingId, string startState)
        {
            this.allValidStates = regularStates.Concat(emergencyStates).ToArray();

            // set current state
            if (!regularStates.Contains(startState))
            {
                throw new System.ArgumentException("Argument Exception: BuildingController can only be initialised to the following states 'open', 'closed', 'out of hours'");
            }

            // set building id
            SetBuildingId(buildingId);

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
        public void SetBuildingId(string buildingId)
        {
            // convert to lowercase
            buildingId = buildingId.ToLower();

            // convert building id to int
            this.buildingId = buildingId;
        }

        // get building id
        public string? GetBuildingId()
        {
            return buildingId;
        }

        // get current state
        public string? GetCurrentState()
        {
            return currentState;
        }

        // set current state
        public bool SetCurrentState(string state)
        {
            // set state to lowercase
            state = state.ToLower();

            // if invalid state
            if (!allValidStates.Contains(state)) { return false; }

            // Remember the previous state before changing to a new state
            if (!emergencyStates.Contains(state))
            {
                previousState = currentState;
            }

            // if current state is in emergency, then do not change the state unless it is reverting to history state
            if (emergencyStates.Contains(currentState) && state != previousState)
            {
                return false;
            }

            currentState = state;
            return true;
        }
    }
}