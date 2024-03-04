using NSubstitute;
using NUnit.Framework;
using SmartBuilding;

namespace SmartBuildingTests
{
    [TestFixture]
    public class LightManagerTests
    {
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
