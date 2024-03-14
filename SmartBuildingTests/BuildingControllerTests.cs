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
        [TestCase("fire drill", true, Category = "Level_One_Tests")]
        [TestCase("fire alarm", true, Category = "Level_One_Tests")]
        [TestCase("clsed", false, Category = "Level_One_Tests")]
        [TestCase("FIRE ALARM", false, Category = "Level_One_Tests")]
        [TestCase("should be invalid", false, Category = "Level_One_Tests")]
        [TestCase("bkPoQyBXzedZ/Ggf", false, Category = "Level_One_Tests")]
        public void SetCurrentState_WithDifferentStates_OnlyAllowsValid(string state, bool expectedOutcome)
        {
            // Arrange
            var controller = new BuildingController("id");

            // Act
            var result = controller.SetCurrentState(state);

            // Assert
            Assert.AreEqual(expectedOutcome, result);
        }
    }
}