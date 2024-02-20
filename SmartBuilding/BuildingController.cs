namespace SmartBuilding
{
    public class BuildingController
    {
        //Write BuildingController code here...
        // define building variables
        private string? buildingId;
        private string? currentState;
        private string? previousState;

        // constructor
        public BuildingController(string buildingId)
        {
            // set building id
            SetBuildingId(buildingId);
            this.currentState = "out of hours";
            this.previousState = this.currentState;
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
            string[] validStates = { "closed", "out of hours", "open", "fire drill", "fire alarm" };

            // Special handling for 'H' to return to the previous state
            if (state == "H")
            {
                currentState = previousState;
                return true;
            }

            if (Array.IndexOf(validStates, state) != -1)
            {
                // Remember the previous state before changing to a new state
                if (currentState != "fire alarm")
                {
                    previousState = currentState;
                }

                currentState = state;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}