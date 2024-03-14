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
        public readonly ILightManager? iLightManager;
        public readonly IFireAlarmManager? iFireAlarmManager;
        public readonly IDoorManager? iDoorManager;
        public readonly IWebService? iWebService;
        public readonly IEmailService? iEmailService;

        public BuildingController(string id)
        {
            // set building id
            SetBuildingID(id);
            this.currentState = "out of hours";
            this.historyState = this.currentState;
            this.allValidStates = regularStates.Concat(emergencyStates).ToArray();
        }

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
            this.currentState = startState;
            this.historyState = this.currentState;
        }

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
            this.currentState = "out of hours";
            this.historyState = this.currentState;
        }

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

            List<string> faultyItems = new();

            if (lightStatus.Contains("FAULT"))
            {
                faultyItems.Add("Lights");
            }
            if (doorStatus.Contains("FAULT"))
            {
                faultyItems.Add("Doors");
            }
            if (fireAlarmStatus.Contains("FAULT"))
            {
                faultyItems.Add("FireAlarm");
            }

            // If there are any faulty items, join them with commas and log the engineer required.
            if (faultyItems.Any())
            {
                iWebService?.LogEngineerRequired(string.Join(",", faultyItems) + ",");
            }

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

            switch (incomingState)
            {
                case "open":
                    if (iDoorManager != null)
                    {
                        bool openedDoors = iDoorManager.OpenAllDoors();

                        // if doors did not open
                        if (!openedDoors) { return false; }
                    }

                    break;

                case "closed":
                    if (iDoorManager != null && iLightManager != null)
                    {
                        bool lockedDoors = iDoorManager.LockAllDoors();
                        iLightManager.SetAllLights(false);

                        // if doors did not lock
                        if (!lockedDoors) { return false; }
                    }

                    break;

                case "fire alarm":
                    if (iFireAlarmManager != null && iDoorManager != null && iLightManager != null && iWebService != null)
                    {
                        iFireAlarmManager.SetAlarm(true);
                        iLightManager.SetAllLights(true);

                        // if exception thrown
                        try
                        {
                            iWebService.LogFireAlarm("fire alarm");
                        }
                        catch (System.Exception e)
                        {
                            string exceptionMessage = e.Message;

                            iEmailService?.SendMail("smartbuilding@uclan.ac.uk", "failed to log alarm", exceptionMessage);
                        }

                        iDoorManager.OpenAllDoors();

                        // if doors did not open
                        if (!iDoorManager.OpenAllDoors()) { return false; }
                    }

                    break;

                default:
                    break;
            }

            // Local dictionary for state transitions
            /*  Valid transitions:
                closed -> out of hours, fire alarm, fire drill
                out of hours -> closed, open, fire alarm, fire drill
                open -> out of hours, fire alarm, fire drill
                fire drill -> history
                fire alarm -> history
            */
            var transitions = new Dictionary<string, string[]> {
                { "closed", new[] { "out of hours", "fire alarm", "fire drill" } },
                { "out of hours", new[] { "closed", "open", "fire alarm", "fire drill" } },
                { "open", new[] { "out of hours", "fire alarm", "fire drill" } },
                { "fire drill", new[] { "history" } },
                { "fire alarm", new[] { "history" } },
            };

            // if current state is in list of emergency states
            if (emergencyStates.Contains(currentState) && regularStates.Contains(incomingState))
            {
                // check if the history state is not null
                if (historyState != null)
                {
                    if (incomingState == historyState)
                    {
                        currentState = incomingState;
                        return true;
                    }
                }
            }

            // Check if the current state allows transitioning to the new state
            if (transitions[currentState].Contains(incomingState))
            {
                // Update historyState before changing currentState
                historyState = currentState;
                currentState = incomingState;

                return true;
            }

            return false;
        }
    }
}