namespace SmartBuilding
{
    public class BuildingController
    {
        //Write BuildingController code here...
        // define building variables
        private string? buildingId;
        private string? currentState;

        // constructor
        public BuildingController(string buildingId)
        {
            // set building id
            SetBuildingId(buildingId);
            currentState = "out of hours";
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
            // make a list of valid states to compare to
            string[] validStates = { "closed", "out of hours", "open", "fire drill", "fire alarm" };

            // check if state is valid
            if (Array.IndexOf(validStates, state) != -1)
            {
                // set current state
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