using ApartmentBook.MVC.Features.Apartments.Models;
using ApartmentBook.MVC.Features.Apartments.Repositories;
using ApartmentBook.MVC.Features.Apartments.Services;
using Moq;

namespace ApartmentBook.MVC.Tests.Features.Apartments.Services
{
    public class ApartmentServiceTests
    {
        private Mock<IApartmentRepository> apartmentRepositoryMock;

        [SetUp]
        public void Setup()
        {
            apartmentRepositoryMock = new Mock<IApartmentRepository>();
        }

        // To fix
        [Test]
        public async Task ShouldReturnApartment()
        {
            // Arrange
            var apartmentId = Guid.NewGuid();
            var apartment = new Apartment();
            apartmentRepositoryMock.Setup(a => a.GetAsync(apartmentId)).ReturnsAsync(apartment);
            var service = CreateService();

            // Act
            var result = await service.GetAsync(apartmentId);

            // Assert
            Assert.That(result, Is.EqualTo(apartment));
        }

        private ApartmentService CreateService()
        {
            return new ApartmentService(apartmentRepositoryMock.Object);
        }
    }
}