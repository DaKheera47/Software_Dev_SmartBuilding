namespace SmartBuilding
{
    public class BuildingController
    {
        //Write BuildingController code here...
        // define building variables
        private string? buildingId;

        // constructor
        public BuildingController(string buildingId)
        {
            // set building id
            SetBuildingId(buildingId);
        }

        // set building id
        public void SetBuildingId(string buildingId)
        {
            // convert building id to int
            this.buildingId = buildingId;
        }

        // get building id
        public string? GetBuildingId()
        {
            return buildingId;
        }

    }
}