namespace SmartBuildingTests;

public interface ILightManager
{
    // Method to set the state of a single light
    void SetLight(bool isOn, int lightId);

    // Method to set the state of all lights
    void SetAllLights(bool isOn);

    // Method to get the status of all managed lights
    string GetStatus();
}
