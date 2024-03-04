using NSubstitute;
using NUnit.Framework;
using SmartBuilding;

namespace SmartBuildingTests
{
    [TestFixture]
    public class Level_Three_Tests
    {
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

        [Test]
        public void GetStatus_AllLightsOk_ReturnsAllOkStatus()
        {
            // Arrange
            var lightManager = Substitute.For<ILightManager>();
            lightManager.GetStatus().Returns("Lights,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK,");

            // Act
            var status = lightManager.GetStatus();

            // Assert
            Assert.AreEqual("Lights,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK,", status);
        }

        [Test]
        public void GetStatus_SomeLightsFaulty_ReturnsCorrectStatus()
        {
            // Arrange
            var lightManager = Substitute.For<ILightManager>();
            lightManager.GetStatus().Returns("Lights,OK,OK,FAULT,OK,OK,OK,OK,FAULT,OK,OK,");

            // Act
            var status = lightManager.GetStatus();

            // Assert
            Assert.AreEqual("Lights,OK,OK,FAULT,OK,OK,OK,OK,FAULT,OK,OK,", status);
        }
    }
}
