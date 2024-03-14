using NUnit.Framework;
using NSubstitute;
using SmartBuilding;

namespace SmartBuildingTests
{
    [TestFixture]
    public class BuildingControllerTests
    {
        //This is a test to check if the project template is correctly configured on your machine
        //you should uncomment the test method below and check that the test appears in the text explorer. (Test -> Test Explorer)
        //When you run the unit test it should pass with the message "Example Test Passed"
        //If the test is not visible or the unit test does not run or pass: Ask a tutor for help
        //When you have confirmed that the template can run unit tests, you can delete the teet. (and this comment)
        [Test]
        public void TemplateTest()
        {
            Assert.Pass("Example Test Passed!");
        }

        //use the following naming convention for your test method names MethodBeingTested_TestScenario_ExpectedOutput
        //E.g. SetCurrentState_InvalidState_ReturnsFalse

        //Write Test Code Here...
        // -- Level 1 Tests --
        // L1R1/L1R3
        [TestCase("abc123", "abc123", Category = "Level_One_Tests")]
        [TestCase("ABC123", "abc123", Category = "Level_One_Tests")]
        [TestCase("123", "123", Category = "Level_One_Tests")]
        public void Constructor_SetsDifferentIDs_SetsBuildingID(string id, string expectedId)
        {
            // Arrange & Act
            BuildingController controller = new(id);

            // Assert
            Assert.AreEqual(expectedId, controller.GetBuildingID());
        }

        // L1R2
        [TestCase("abc123", "abc123", Category = "Level_One_Tests")]
        [TestCase("ABC123", "abc123", Category = "Level_One_Tests")]
        [TestCase("123", "123", Category = "Level_One_Tests")]
        public void GetBuildingId_AfterConstruction_ReturnsId(string id, string expectedId)
        {
            // Arrange
            BuildingController controller = new(id);

            // Act
            string? result = controller.GetBuildingID();

            // Assert
            Assert.AreEqual(expectedId, result);
        }

        // L1R4
        [TestCase("abc123", "abc123", Category = "Level_One_Tests")]
        [TestCase("ABC123", "abc123", Category = "Level_One_Tests")]
        [TestCase("123", "123", Category = "Level_One_Tests")]
        public void SetBuildingId_WithUppercaseId_SetsLowercaseBuildingId(string id, string expectedId)
        {
            // Arrange
            BuildingController controller = new("testing id");

            // Act
            controller.SetBuildingID(id);

            // Assert
            Assert.AreEqual(id.ToLower(), controller.GetBuildingID());
        }

        // L1R5
        [TestCase("abc123", Category = "Level_One_Tests")]
        [TestCase("ABC123", Category = "Level_One_Tests")]
        [TestCase("123", Category = "Level_One_Tests")]
        public void Constructor_SetsCurrentStateToOutOfHours(string id)
        {
            // Arrange
            BuildingController controller = new(id);

            // Act
            string state = controller.GetCurrentState();

            // Assert
            Assert.AreEqual("out of hours", state);
        }

        // L1R6
        [TestCase("abc123", Category = "Level_One_Tests")]
        [TestCase("ABC123", Category = "Level_One_Tests")]
        [TestCase("123", Category = "Level_One_Tests")]
        public void GetCurrentState_WhenCalled_ReturnsCurrentState(string id)
        {
            // Arrange
            BuildingController controller = new(id);

            // Act
            string state = controller.GetCurrentState();

            // Assert
            Assert.AreEqual("out of hours", state);
        }

        // L1R7
        [TestCase("closed", true, Category = "Level_One_Tests")]
        [TestCase("out of hours", true, Category = "Level_One_Tests")]
        [TestCase("open", true, Category = "Level_One_Tests")]
        [TestCase("fire drill", true, Category = "Level_One_Tests")]
        [TestCase("fire alarm", true, Category = "Level_One_Tests")]
        [TestCase("clsed", false, Category = "Level_One_Tests")]
        [TestCase("CLOSED", true, Category = "Level_One_Tests")]
        [TestCase("should be invalid", false, Category = "Level_One_Tests")]
        [TestCase("bkPoQyBXzedZ/Ggf", false, Category = "Level_One_Tests")]
        [TestCase("RI2GV9ubBqOIev+7", false, Category = "Level_One_Tests")]
        public void SetCurrentState_WithDifferentStates_OnlyAllowsValid(string state, bool expectedOutcome)
        {
            // Arrange
            BuildingController controller = new("id");

            // Act
            bool result = controller.SetCurrentState(state);

            // Assert
            Assert.AreEqual(expectedOutcome, result);
        }

        // -- Level 2 Tests --
        // L2R1
        // valid cases
        [TestCase("out of hours", "fire drill", "out of hours", true)]
        [TestCase("closed", "fire drill", "closed", true)]
        [TestCase("closed", "fire drill", "fire drill", true)]
        [TestCase("closed", "fire alarm", "fire alarm", true)]
        [TestCase("closed", "fire alarm", "closed", true)]
        [TestCase("open", "fire drill", "open", true)]
        [TestCase("open", "fire alarm", "open", true)]
        [TestCase("open", "fire drill", "fire drill", true)]
        [TestCase("open", "fire alarm", "fire alarm", true)]
        [TestCase("out of hours", "fire drill", "fire drill", true)]
        [TestCase("out of hours", "fire alarm", "out of hours", true)]
        // failure cases
        [TestCase("out of hours", "fire drill", "closed", false)]
        [TestCase("out of hours", "fire drill", "open", false)]
        [TestCase("out of hours", "fire drill", "fire alarm", false)]
        [TestCase("out of hours", "fire alarm", "closed", false)]
        [TestCase("out of hours", "fire alarm", "open", false)]
        [TestCase("out of hours", "fire alarm", "fire drill", false)]
        [TestCase("out of hours", "fire alarm", "fire alarm", true)]
        [TestCase("closed", "fire drill", "out of hours", false)]
        [TestCase("closed", "fire drill", "open", false)]
        [TestCase("closed", "fire drill", "fire alarm", false)]
        [TestCase("closed", "fire alarm", "out of hours", false)]
        [TestCase("closed", "fire alarm", "open", false)]
        [TestCase("closed", "fire alarm", "fire drill", false)]
        [TestCase("open", "fire drill", "out of hours", false)]
        [TestCase("open", "fire drill", "closed", false)]
        [TestCase("open", "fire drill", "fire alarm", false)]
        [TestCase("open", "fire alarm", "out of hours", false)]
        [TestCase("open", "fire alarm", "closed", false)]
        [TestCase("open", "fire alarm", "fire drill", false)]

        public void SetCurrentState_StateTransition_OnlyAllowsValid_WithHistory(string historyState, string fromState, string toState, bool expectedOutcome)
        {
            // Arrange
            BuildingController controller = new("id");

            // Act
            controller.SetCurrentState(historyState);
            controller.SetCurrentState(fromState);
            bool result = controller.SetCurrentState(toState);

            // Assert
            Assert.AreEqual(expectedOutcome, result);
        }

        // L2R1
        [TestCase("open", "out of hours", true)]
        [TestCase("open", "closed", false)]
        [TestCase("open", "open", true)]
        [TestCase("open", "fire drill", true)]
        [TestCase("open", "fire alarm", true)]
        [TestCase("closed", "out of hours", true)]
        [TestCase("closed", "closed", true)]
        [TestCase("closed", "open", false)]
        [TestCase("closed", "fire drill", true)]
        [TestCase("closed", "fire alarm", true)]
        [TestCase("out of hours", "out of hours", true)]
        [TestCase("out of hours", "closed", true)]
        [TestCase("out of hours", "open", true)]
        [TestCase("out of hours", "fire drill", true)]
        [TestCase("out of hours", "fire alarm", true)]
        public void SetCurrentState_StateTransition_OnlyAllowsValid(string fromState, string toState, bool expectedOutcome)
        {
            // Arrange
            BuildingController controller = new("id");

            // Act
            controller.SetCurrentState(fromState);
            bool result = controller.SetCurrentState(toState);

            // Assert
            Assert.AreEqual(expectedOutcome, result);
        }

        // L2R2
        [TestCase("open")]
        [TestCase("closed")]
        [TestCase("out of hours")]
        public void SetCurrentState_ToSameState_ReturnsTrue(string state)
        {
            // Arrange
            BuildingController controller = new("id");

            // Act
            controller.SetCurrentState(state);
            bool result = controller.SetCurrentState(state);

            // Assert
            Assert.IsTrue(result);
        }

        // L2R3
        [TestCase("abc123", "abc123", "open", "open")]
        [TestCase("abc123", "abc123", "closed", "closed")]
        [TestCase("abc123", "abc123", "out of hours", "out of hours")]
        [TestCase("ABC123", "abc123", "open", "open")]
        [TestCase("123", "123", "open", "open")]
        [TestCase("", "", "open", "open")]
        [TestCase("Ry/4otmctLgdJ0pW", "ry/4otmctlgdj0pw", "open", "open")]
        public void Constructor_WithValidStartState_SetsState(string id, string expectedID, string startState, string expectedState)
        {
            // Arrange & Act
            BuildingController controller = new(id, startState);

            // Assert
            Assert.AreEqual(expectedID, controller.GetBuildingID());
            Assert.AreEqual(expectedState, controller.GetCurrentState());
        }

        // L2R3
        [TestCase("open", false)]
        [TestCase("closed", false)]
        [TestCase("out of hours", false)]
        [TestCase("fire drill", true)]
        [TestCase("fire alarm", true)]
        public void Constructor_WithInvalidStartState_DoesntSetState(string startState, bool willThrowException)
        {
            if (willThrowException)
            {
                // Arrange & Act
                // make sure the exception is thrown
                System.ArgumentException? ex = Assert.Throws<ArgumentException>(() => new BuildingController("id", startState));

                if (ex != null)
                {
                    // Assert
                    // make sure the message is correct
                    Assert.AreEqual("Argument Exception: BuildingController can only be initialised to the following states 'open', 'closed', 'out of hours'", ex.Message);
                }

                return;
            }

            // Arrange
            string expectedState = startState;

            // Arrange & Act
            BuildingController controller = new("id", startState);

            // Assert
            Assert.AreEqual(expectedState, controller.GetCurrentState());
        }

        // -- Level 3 Tests --
        // L3R1
        [Test]
        public void Constructor_SettingManagers_SetsManagersCorrectly()
        {
            // Arrange
            ILightManager lightManager = Substitute.For<ILightManager>();
            IDoorManager doorManager = Substitute.For<IDoorManager>();
            IFireAlarmManager fireAlarmManager = Substitute.For<IFireAlarmManager>();
            IWebService webService = Substitute.For<IWebService>();
            IEmailService emailService = Substitute.For<IEmailService>();

            // Act
            BuildingController controller = new("id", lightManager, fireAlarmManager, doorManager, webService, emailService);

            // Assert
            Assert.AreEqual(lightManager, controller.iLightManager);
            Assert.AreEqual(doorManager, controller.iDoorManager);
            Assert.AreEqual(fireAlarmManager, controller.iFireAlarmManager);
            Assert.AreEqual(webService, controller.iWebService);
            Assert.AreEqual(emailService, controller.iEmailService);
        }

        // L3R3
        [TestCase("Lights,OK,OK,OK,", "Doors,OK,OK,OK,", "FireAlarm,OK,OK,OK,", "Lights,OK,OK,OK,Doors,OK,OK,OK,FireAlarm,OK,OK,OK,")]
        [TestCase("Lights,FAULT,OK,OK,", "Doors,OK,FAULT,OK,", "FireAlarm,FAULT,OK,OK,", "Lights,FAULT,OK,OK,Doors,OK,FAULT,OK,FireAlarm,FAULT,OK,OK,")]
        [TestCase("Lights,OK,FAULT,OK,FAULT,", "Doors,FAULT,OK,FAULT,OK,", "FireAlarm,OK,FAULT,OK,FAULT,", "Lights,OK,FAULT,OK,FAULT,Doors,FAULT,OK,FAULT,OK,FireAlarm,OK,FAULT,OK,FAULT,")]
        [TestCase("Lights,OK,FAULT,OK,", "Doors,FAULT,OK,FAULT,", "FireAlarm,OK,OK,FAULT,", "Lights,OK,FAULT,OK,Doors,FAULT,OK,FAULT,FireAlarm,OK,OK,FAULT,")]
        [TestCase("Lights,FAULT,OK,OK,", "Doors,OK,FAULT,OK,", "FireAlarm,FAULT,OK,OK,", "Lights,FAULT,OK,OK,Doors,OK,FAULT,OK,FireAlarm,FAULT,OK,OK,")]
        public void GetStatusReport_ConcatenatesManagerOutputs_ReturnsConcatenatedString(string lightStatus, string doorStatus, string fireAlarmStatus, string expectedStatusReport)
        {
            // Arrange
            ILightManager lightManager = Substitute.For<ILightManager>();
            IDoorManager doorManager = Substitute.For<IDoorManager>();
            IFireAlarmManager fireAlarmManager = Substitute.For<IFireAlarmManager>();
            IWebService webService = Substitute.For<IWebService>();
            IEmailService emailService = Substitute.For<IEmailService>();

            // Set up the manager outputs
            lightManager.GetStatus().Returns(lightStatus);
            doorManager.GetStatus().Returns(doorStatus);
            fireAlarmManager.GetStatus().Returns(fireAlarmStatus);

            // Set up the controller with the managers
            BuildingController controller = new("id", lightManager, fireAlarmManager, doorManager, webService, emailService);

            // Act
            string statusReport = controller.GetStatusReport();

            // Assert
            Assert.AreEqual(expectedStatusReport, statusReport);
        }

        // L3R4/L3R5
        [TestCase(true, true, "open")]
        [TestCase(false, false, "out of hours")]
        public void SetCurrentState_OpensAllDoors_ReturnsTrueIfDoorsCanOpen(bool doorOpenResult, bool expectedSetStateResult, string expectedStateAfterOpenAttempt)
        {
            // Arrange
            ILightManager lightManager = Substitute.For<ILightManager>();
            IDoorManager doorManager = Substitute.For<IDoorManager>();
            IFireAlarmManager fireAlarmManager = Substitute.For<IFireAlarmManager>();
            IWebService webService = Substitute.For<IWebService>();
            IEmailService emailService = Substitute.For<IEmailService>();
            doorManager.OpenAllDoors().Returns(doorOpenResult);

            BuildingController controller = new("id", lightManager, fireAlarmManager, doorManager, webService, emailService);

            // Act
            bool result = controller.SetCurrentState("open");

            // Assert
            Assert.AreEqual(expectedSetStateResult, result);
            Assert.AreEqual(expectedStateAfterOpenAttempt, controller.GetCurrentState());
            // Check that the doors were attempted to open
            doorManager.Received().OpenAllDoors();
        }

        // -- Level 4 Tests --
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
