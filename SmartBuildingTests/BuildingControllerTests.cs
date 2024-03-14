using NUnit.Framework;
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
        // L1R1/L1R3
        [TestCase("abc123", "abc123", Category = "Level_One_Tests")]
        [TestCase("ABC123", "abc123", Category = "Level_One_Tests")]
        [TestCase("123", "123", Category = "Level_One_Tests")]
        public void Constructor_SetsDifferentIDs_SetsBuildingID(string id, string expectedId)
        {
            // Arrange & Act
            BuildingController controller = new BuildingController(id);

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
            BuildingController controller = new BuildingController(id);

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
            var controller = new BuildingController("testing id");

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
            var controller = new BuildingController(id);

            // Act
            var state = controller.GetCurrentState();

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
            var controller = new BuildingController(id);

            // Act
            var state = controller.GetCurrentState();

            // Assert
            Assert.AreEqual("out of hours", state);
        }

        // L1R7
        [TestCase("closed", true, Category = "Level_One_Tests")]
        [TestCase("out of hours", true, Category = "Level_One_Tests")]
        [TestCase("open", true, Category = "Level_One_Tests")]
        // [TestCase("fire drill", true, Category = "Level_One_Tests")]
        // [TestCase("fire alarm", true, Category = "Level_One_Tests")]
        [TestCase("clsed", false, Category = "Level_One_Tests")]
        [TestCase("CLOSED", false, Category = "Level_One_Tests")]
        [TestCase("should be invalid", false, Category = "Level_One_Tests")]
        [TestCase("bkPoQyBXzedZ/Ggf", false, Category = "Level_One_Tests")]
        [TestCase("RI2GV9ubBqOIev+7", false, Category = "Level_One_Tests")]
        public void SetCurrentState_WithDifferentStates_OnlyAllowsValid(string state, bool expectedOutcome)
        {
            // Arrange
            var controller = new BuildingController("id");

            // Act
            var result = controller.SetCurrentState(state);

            // Assert
            Assert.AreEqual(expectedOutcome, result);
        }

        // L2R1
        [TestCase("open", "out of hours", true)]
        [TestCase("closed", "out of hours", true)]
        [TestCase("out of hours", "open", true)]
        [TestCase("out of hours", "closed", true)]
        [TestCase("open", "closed", false)]
        [TestCase("closed", "open", false)]
        public void SetCurrentState_StateTransition_OnlyAllowsValid(string fromState, string toState, bool expectedOutcome)
        {
            // Arrange
            var controller = new BuildingController("id");

            // Act
            controller.SetCurrentState(fromState);
            var result = controller.SetCurrentState(toState);

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
            var controller = new BuildingController("id");

            // Act
            controller.SetCurrentState(state);
            var result = controller.SetCurrentState(state);

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
            var controller = new BuildingController(id, startState);

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
                var ex = Assert.Throws<ArgumentException>(() => new BuildingController("id", startState));

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
            var controller = new BuildingController("id", startState);

            // Assert
            Assert.AreEqual(expectedState, controller.GetCurrentState());
        }
    }
}
