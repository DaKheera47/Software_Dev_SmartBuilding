using NSubstitute;
using NUnit.Framework;
using SmartBuilding;

namespace SmartBuildingTests
{
    [TestFixture]
    public class Level_Three_Tests
    {
        // L3R5
        [TestCase(true, true, "open", TestName = "SetCurrentState_Open_SuccessfullyOpensDoors_ResultsInOpenState")]
        [TestCase(false, false, "closed", TestName = "SetCurrentState_Open_FailsToOpenDoors_StateRemainsClosed")]
        public void SetCurrentState_Open_VariesByDoorManagerResponse(bool doorOpenResult, bool expectedSetStateResult, string expectedStateAfterOpenAttempt)
        {
            // Arrange
            var lightManager = Substitute.For<ILightManager>();
            var doorManager = Substitute.For<IDoorManager>();
            var fireAlarmManager = Substitute.For<IFireAlarmManager>();
            var webService = Substitute.For<IWebService>();
            var emailService = Substitute.For<IEmailService>();
            var buildingController = new BuildingController("id", lightManager, fireAlarmManager, doorManager, webService, emailService);

            buildingController.SetCurrentState("out of hours"); // Ensure the initial state is closed before attempting to open
            doorManager.OpenAllDoors().Returns(doorOpenResult);

            // Act
            var result = buildingController.SetCurrentState("open");

            // Assert
            Assert.AreEqual(expectedSetStateResult, result, "Unexpected result from SetCurrentState when opening.");
            Assert.AreEqual(expectedStateAfterOpenAttempt, buildingController.GetCurrentState(), "Unexpected final state after attempt to open.");
        }

        // L3R4
        [TestCase(true, true, "open")]
        [TestCase(false, false, "open")]
        public void SetCurrentState_ToOpen_CallsDoorManagerAndSetsStateAccordingly(bool doorOpenResult, bool expectedSetStateResult, string expectedStateAfterOpenAttempt)
        {
            // Arrange
            var lightManager = Substitute.For<ILightManager>();
            var doorManager = Substitute.For<IDoorManager>();
            var fireAlarmManager = Substitute.For<IFireAlarmManager>();
            var webService = Substitute.For<IWebService>();
            var emailService = Substitute.For<IEmailService>();
            var buildingController = new BuildingController("id", lightManager, fireAlarmManager, doorManager, webService, emailService);

            // starting from state being "closed"
            buildingController.SetCurrentState("out of hours");
            doorManager.OpenAllDoors().Returns(doorOpenResult);

            // Act
            var result = buildingController.SetCurrentState("open");

            // Assert
            Assert.AreEqual(expectedSetStateResult, result, $"Expected SetCurrentState to return {expectedSetStateResult}.");
            Assert.AreEqual(expectedStateAfterOpenAttempt, buildingController.GetCurrentState(), $"Expected the state to be {expectedStateAfterOpenAttempt}.");
        }

        // L3R3
        [TestCase("Lights,OK,OK,OK,", "Doors,OK,OK,OK,", "FireAlarm,OK,OK,OK,", "Lights,OK,OK,OK,Doors,OK,OK,OK,FireAlarm,OK,OK,OK,")]
        [TestCase("Lights,FAULT,OK,OK,", "Doors,OK,FAULT,OK,", "FireAlarm,FAULT,OK,OK,", "Lights,FAULT,OK,OK,Doors,OK,FAULT,OK,FireAlarm,FAULT,OK,OK,")]
        public void GetStatusReport_CombinesManagerStatusesIntoSingleString(string lightStatus, string doorStatus, string fireAlarmStatus, string expectedStatusReport)
        {
            // Arrange
            var lightManager = Substitute.For<ILightManager>();
            var doorManager = Substitute.For<IDoorManager>();
            var fireAlarmManager = Substitute.For<IFireAlarmManager>();
            var webService = Substitute.For<IWebService>();
            var emailService = Substitute.For<IEmailService>();
            var buildingController = new BuildingController("id", lightManager, fireAlarmManager, doorManager, webService, emailService);

            lightManager.GetStatus().Returns(lightStatus);
            doorManager.GetStatus().Returns(doorStatus);
            fireAlarmManager.GetStatus().Returns(fireAlarmStatus);

            // Act
            var statusReport = buildingController.GetStatusReport();

            // Assert
            Assert.AreEqual(expectedStatusReport, statusReport);
        }

        // L3R2
        [TestCase("Lights,OK,OK,OK,")]
        [TestCase("Lights,FAULT,OK,OK,")]
        public void GetStatus_AllLightsOk_ReturnsAllOkStatus(string possibleStatus)
        {
            // Arrange
            var lightManager = Substitute.For<ILightManager>();
            lightManager.GetStatus().Returns(possibleStatus);

            // Act
            var status = lightManager.GetStatus();

            // Assert
            Assert.AreEqual(possibleStatus, status);
        }
    }
}
