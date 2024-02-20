using NUnit.Framework;
using SmartBuilding;

namespace SmartBuildingTests
{
    [TestFixture]
    public class Level_One_Tests
    {
        // L1R1 and L1R3: Test constructor assigns buildingID with lowercase conversion
        [Test]
        public void Constructor_WhenCalledWithUppercaseId_AssignsLowercaseBuildingId()
        {
            // Arrange & Act
            var controller = new BuildingController("ABC123");

            // Assert
            Assert.AreEqual("abc123", controller.GetBuildingId());
        }

        // L1R2: Test GetBuildingID returns the correct value
        [Test]
        public void GetBuildingId_AfterConstruction_ReturnsCorrectId()
        {
            // Arrange
            var expectedId = "building1";
            var controller = new BuildingController(expectedId);

            // Act
            var result = controller.GetBuildingId();

            // Assert
            Assert.AreEqual(expectedId, result);
        }

        // L1R4: Test SetBuildingID sets the buildingID with lowercase conversion
        [Test]
        public void SetBuildingId_WhenCalledWithUppercaseId_SetsLowercaseBuildingId()
        {
            // Arrange
            var controller = new BuildingController("initialId");
            var newId = "NEWID";

            // Act
            controller.SetBuildingId(newId);

            // Assert
            Assert.AreEqual(newId.ToLower(), controller.GetBuildingId());
        }

        // L1R5: Test constructor sets currentState to “out of hours”
        [Test]
        public void Constructor_WhenCalled_SetsCurrentStateToOutOfHoursInitially()
        {
            // Arrange & Act
            var controller = new BuildingController("id");

            // Assert
            Assert.AreEqual("out of hours", controller.GetCurrentState());
        }

        // L1R6: Test GetCurrentState returns the current state
        [Test]
        public void GetCurrentState_WhenCalled_ReturnsCurrentState()
        {
            // Arrange
            var controller = new BuildingController("id");

            // Act
            var state = controller.GetCurrentState();

            // Assert
            Assert.AreEqual("out of hours", state);
        }

        // L1R7: Test SetCurrentState with valid and invalid states
        [TestCase("closed", true)]
        [TestCase("out of hours", true)]
        [TestCase("open", true)]
        [TestCase("fire drill", true)]
        [TestCase("fire alarm", true)]
        [TestCase("should be invalid", false)]
        public void SetCurrentState_WithDifferentStates_SetsStateAccordingly(string state, bool expectedOutcome)
        {
            // Arrange
            var controller = new BuildingController("id");

            // Act
            var result = controller.SetCurrentState(state);

            Assert.AreEqual(expectedOutcome, result);
            if (expectedOutcome)
            {
                Assert.AreEqual(state, controller.GetCurrentState());
            }
        }
    }
}