namespace SmartBuilding;

public interface IWebService
{
    // Method to get the status
    void LogFireAlarm(string logDetails);

    void LogEngineerRequired(string logDetails);

    
}
