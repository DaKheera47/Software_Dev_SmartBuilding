using NUnit.Framework;
using NSubstitute;
using SmartBuilding;

namespace SmartBuildingTests
{
    [TestFixture]
    public class Level_Four_Tests
    {
        // L4R1
        [Test]
        public void SetCurrentState_GoingToClosed_LocksDoors()
        {
            // Arrange
            ILightManager lightManager = Substitute.For<ILightManager>();
            IDoorManager doorManager = Substitute.For<IDoorManager>();
            IFireAlarmManager fireAlarmManager = Substitute.For<IFireAlarmManager>();
            IWebService webService = Substitute.For<IWebService>();
            IEmailService emailService = Substitute.For<IEmailService>();

            // set up door manager
            doorManager.LockAllDoors().Returns(true);

            // set up light manager
            lightManager.SetAllLights(Arg.Any<bool>());

            BuildingController controller = new("id", lightManager, fireAlarmManager, doorManager, webService, emailService);

            // Act
            bool result = controller.SetCurrentState("closed");

            // Assert
            // Check that the doors were attempted to lock
            doorManager.Received().LockAllDoors();

            // make sure lights were turned off
            lightManager.Received().SetAllLights(false);

            // make sure the result is true, ie out of hours to closed happened
            Assert.AreEqual(true, result);
        }

        // L4R2
        [Test]
        public void SetCurrentState_ToFireAlarm_AlarmTriggered()
        {
            // Arrange
            ILightManager lightManager = Substitute.For<ILightManager>();
            IDoorManager doorManager = Substitute.For<IDoorManager>();
            IFireAlarmManager fireAlarmManager = Substitute.For<IFireAlarmManager>();
            IWebService webService = Substitute.For<IWebService>();
            IEmailService emailService = Substitute.For<IEmailService>();

            // set up door manager to return true when all doors are opened
            doorManager.OpenAllDoors().Returns(true);

            BuildingController controller = new("id", lightManager, fireAlarmManager, doorManager, webService, emailService);

            // Act
            bool result = controller.SetCurrentState("fire alarm");

            // Assert
            // firealarm was set
            fireAlarmManager.Received().SetAlarm(true);
            // doors were unlocked
            doorManager.Received().OpenAllDoors();
            // lights were on
            lightManager.Received().SetAllLights(true);
            // online log was called with "fire alarm"
            webService.Received().LogFireAlarm("fire alarm");
        }
    }
}