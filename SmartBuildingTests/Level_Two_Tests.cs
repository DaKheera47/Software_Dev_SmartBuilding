using NUnit.Framework;
using SmartBuilding;

namespace SmartBuildingTests
{
    [TestFixture]
    public class Level_Two_Tests
    {
        // L2R1: Test SetCurrentState() only allows valid state transitions
        [Test]
        public void SetCurrentState_ToValidTransition_ChangesState()
        {
            // Arrange
            var controller = new BuildingController("id");

            // Act & Assert
            var closedResult = controller.SetCurrentState("closed");
            Assert.IsTrue(closedResult, "Setting state to 'closed' passes.");
            Assert.AreEqual("closed", controller.GetCurrentState(), "Current state is 'closed' as expected.");

            var fireDrillResult = controller.SetCurrentState("fire drill");
            Assert.IsTrue(fireDrillResult, "Setting state to 'fire drill' passes.");
            Assert.AreEqual("fire drill", controller.GetCurrentState(), "Current state is not 'fire drill' as expected.");

            var openResult = controller.SetCurrentState("open");
            Assert.IsFalse(openResult, "Setting state to 'open' should fail but succeeded.");
            Assert.AreEqual("fire drill", controller.GetCurrentState(), "Current state should remain 'fire drill' after failed attempt to change to 'open'.");
        }


        // L2R2: Test SetCurrentState() returns true and keeps the same state if the current state is set again
        [Test]
        public void SetCurrentState_ToSameState_ReturnsTrueAndKeepsState()
        {
            // Arrange
            var controller = new BuildingController("id", "closed");

            // Act & Assert
            Assert.IsTrue(controller.SetCurrentState("closed"));
            Assert.AreEqual("closed", controller.GetCurrentState());
        }

        // L2R3: Test the additional constructor with buildingID and currentState
        [Test]
        public void Constructor_WithValidStartState_SetsState()
        {
            // Arrange & Act
            var controller = new BuildingController("id", "closed");

            // Assert
            Assert.AreEqual("closed", controller.GetCurrentState());
        }

        // L2R3: TheBuildingController class can only be initialised to one of the three normaloperation states (“closed”, “out of hours” or “open”).
        [Test]
        public void Constructor_WithInvalidStartState_ThrowsArgumentException()
        {
            // Arrange, Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => new BuildingController("id", "invalid"));
            Assert.That(exception!.Message, Is.EqualTo("Argument Exception: BuildingController can only be initialised to the following states 'open', 'closed', 'out of hours'"));
        }
    }
}