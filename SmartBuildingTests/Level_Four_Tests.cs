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

        // L4R3
        // all ok
        [TestCase("Lights,OK,OK,OK,", "Doors,OK,OK,OK,", "FireAlarm,OK,OK,OK,", "Lights,OK,OK,OK,Doors,OK,OK,OK,FireAlarm,OK,OK,OK,")]
        // one fault
        [TestCase("Lights,FAULT,OK,OK,", "Doors,OK,OK,OK,", "FireAlarm,OK,OK,OK,", "Lights,FAULT,OK,OK,Doors,OK,OK,OK,FireAlarm,OK,OK,OK,")]
        // one fault, different thing
        [TestCase("Lights,OK,OK,OK,", "Doors,OK,OK,OK,", "FireAlarm,OK,OK,FAULT,", "Lights,OK,OK,OK,Doors,OK,OK,OK,FireAlarm,OK,OK,FAULT,")]
        // multiple faults
        [TestCase("Lights,OK,FAULT,OK,OK,", "Doors,FAULT,OK,OK,OK,", "FireAlarm,OK,OK,OK,OK,", "Lights,OK,FAULT,OK,OK,Doors,FAULT,OK,OK,OK,FireAlarm,OK,OK,OK,OK,")]
        // multiple faults, different things
        [TestCase("Lights,OK,FAULT,OK,OK,", "Doors,FAULT,OK,OK,OK,", "FireAlarm,OK,FAULT,OK,OK,", "Lights,OK,FAULT,OK,OK,Doors,FAULT,OK,OK,OK,FireAlarm,OK,FAULT,OK,OK,")]
        // multiple faults in one thing
        [TestCase("Lights,OK,FAULT,FAULT,OK,", "Doors,OK,OK,OK,OK,", "FireAlarm,OK,OK,OK,OK,", "Lights,OK,FAULT,FAULT,OK,Doors,OK,OK,OK,OK,FireAlarm,OK,OK,OK,OK,")]
        // multiple faults in multiple things
        [TestCase("Lights,OK,FAULT,FAULT,OK,", "Doors,OK,FAULT,OK,FAULT,", "FireAlarm,FAULT,FAULT,FAULT,OK,", "Lights,OK,FAULT,FAULT,OK,Doors,OK,FAULT,OK,FAULT,FireAlarm,FAULT,FAULT,FAULT,OK,")]
        // all faults
        [TestCase("Lights,FAULT,FAULT,FAULT,FAULT,", "Doors,FAULT,FAULT,FAULT,FAULT,", "FireAlarm,FAULT,FAULT,FAULT,FAULT,", "Lights,FAULT,FAULT,FAULT,FAULT,Doors,FAULT,FAULT,FAULT,FAULT,FireAlarm,FAULT,FAULT,FAULT,FAULT,")]
        public void GetStatusReport_ParsingStatusReport_WebServiceCalled(string lightStatus, string doorStatus, string fireAlarmStatus, string expectedStatusReport)
        {
            // Arrange
            ILightManager lightManager = Substitute.For<ILightManager>();
            IDoorManager doorManager = Substitute.For<IDoorManager>();
            IFireAlarmManager fireAlarmManager = Substitute.For<IFireAlarmManager>();
            IWebService webService = Substitute.For<IWebService>();
            IEmailService emailService = Substitute.For<IEmailService>();

            // set up light manager
            lightManager.GetStatus().Returns(lightStatus);
            // set up door manager
            doorManager.GetStatus().Returns(doorStatus);
            // set up fire alarm manager
            fireAlarmManager.GetStatus().Returns(fireAlarmStatus);

            BuildingController controller = new("id", lightManager, fireAlarmManager, doorManager, webService, emailService);

            // Act
            string result = controller.GetStatusReport();

            // Assert
            // make sure the result is as expected
            Assert.AreEqual(expectedStatusReport, result);
            // if no fault detected, no calls to web service's LogEngineerRequired
            if (!lightStatus.Contains("FAULT") && !doorStatus.Contains("FAULT") && !fireAlarmStatus.Contains("FAULT"))
            {
                webService.DidNotReceive().LogEngineerRequired(Arg.Any<string>());
            }

            // if fault detected, call to web service's LogEngineerRequired with the thing that is a fault, so Lights, Doors or FireAlarm
            // if multiple faults detected, call to web service's LogEngineerRequired with the things that are a fault, so Lights, Doors or FireAlarm, comma separated
            // Create a list to hold the faulty items.
            List<string> faultyItems = new();

            if (lightStatus.Contains("FAULT"))
            {
                faultyItems.Add("Lights");
            }
            if (doorStatus.Contains("FAULT"))
            {
                faultyItems.Add("Doors");
            }
            if (fireAlarmStatus.Contains("FAULT"))
            {
                faultyItems.Add("FireAlarm");
            }

            // If there are any faulty items, join them with commas and log the engineer required.
            if (faultyItems.Any())
            {
                webService.Received().LogEngineerRequired(string.Join(",", faultyItems) + ",");
            }
        }

        // L4R4
        [TestCase(true, "Exception Message!")]
        [TestCase(true, "The web service was unable to log the fire alarm.")]
        [TestCase(false, "")]
        public void SetCurrentState_WebServiceThrowsException_EmailSent(bool willWebServiceThrow, string exceptionMessage)
        {
            // Arrange
            ILightManager lightManager = Substitute.For<ILightManager>();
            IDoorManager doorManager = Substitute.For<IDoorManager>();
            IFireAlarmManager fireAlarmManager = Substitute.For<IFireAlarmManager>();
            IWebService webService = Substitute.For<IWebService>();
            IEmailService emailService = Substitute.For<IEmailService>();

            // set up door manager to return true when all doors are opened
            doorManager.OpenAllDoors().Returns(true);

            // set up web service to throw exception if willWebServiceThrow is true
            if (willWebServiceThrow)
            {
                webService.When(x => x.LogFireAlarm("fire alarm")).Do(x => { throw new System.Exception(exceptionMessage); });
            }

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

            // if web service throws exception, email service should be called
            if (willWebServiceThrow)
            {
                emailService.Received().SendMail("smartbuilding@uclan.ac.uk", "failed to log alarm", exceptionMessage);
            }
        }
    }
}