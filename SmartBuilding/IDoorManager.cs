namespace SmartBuilding;

public interface IDoorManager
{
    // Method to get the status
    string GetStatus();

    // Method to open all doors
    bool OpenAllDoors();
}
