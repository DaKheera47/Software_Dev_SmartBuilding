namespace SmartBuilding;

public interface IFireAlarmManager
{
    // Method to get the status
    string GetStatus();

    void SetAlarm(bool alarmStatus);
}
