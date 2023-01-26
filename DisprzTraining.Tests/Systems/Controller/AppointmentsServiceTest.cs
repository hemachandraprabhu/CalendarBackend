using DisprzTraining.Business;
using DisprzTraining.Controllers;
using DisprzTraining.Dtos;
using DisprzTraining.Models;
using DisprzTraining.Tests.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DisprzTraining.Tests.Systems.Controller
{
    public class AppointmentsServiceTest
    {
        private readonly Mock<IAppointmentBL> mockAppointmentBL = new();

        [Fact]
        public async Task GetAppointmentByDateAsync_withValidDate_ReturnsOkResult()
        {
            // Given
            DateTime d = new DateTime();
            mockAppointmentBL.Setup(service => service.GetAppointmentsByDateAsync(It.IsAny<DateTime>())).ReturnsAsync(AppointmentsFixture.GetTestAppointments());
            var sut = new AppointmentsController(mockAppointmentBL.Object);

            // When
            var res = await sut.GetAppointmentsByDateAsync(d);

            // Then
            res.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetAppointmentByDateAsync_withInValidDate_ReturnsNotFound()
        {
            // Given
            DateTime d = new DateTime();
            mockAppointmentBL.Setup(service => service.GetAppointmentsByDateAsync(d)).ReturnsAsync(new List<Appointment>());
            var sut = new AppointmentsController(mockAppointmentBL.Object);

            // When
            var res = await sut.GetAppointmentsByDateAsync(d);

            // Then
            res.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GetAppointmentByMonthAsync_withValidMonth_ReturnsOkResult()
        {
            // Given
            DateTime d = new DateTime();
            mockAppointmentBL.Setup(service => service.GetAppointmentsByMonthAsync(It.IsAny<DateTime>())).ReturnsAsync(AppointmentsFixture.GetTestAppointments());
            var sut = new AppointmentsController(mockAppointmentBL.Object);

            // When
            var res = await sut.GetAppointmentsByMonthAsync(d);

            // Then
            res.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetAppointmentByMonthAsync_withInValidMonth_ReturnsNotFound()
        {
            // Given
            DateTime d = new DateTime();
            mockAppointmentBL.Setup(service => service.GetAppointmentsByMonthAsync(d)).ReturnsAsync(new List<Appointment>());
            var sut = new AppointmentsController(mockAppointmentBL.Object);

            // When
            var res = await sut.GetAppointmentsByMonthAsync(d);

            // Then
            res.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task AddAppointmentAsync_withAppointmentToAdd_ReturnsAddedAppointment()
        {
            /* Arrange */
            PostItemDto AppointmentToAdd = new PostItemDto(new DateTime(2024, 12, 22, 8, 0, 0), new DateTime(2024, 12, 22, 12, 0, 0), "Test");
            Appointment appointment = new()
            {
                id = Guid.NewGuid(),
                startDate = AppointmentToAdd.startDate,
                endDate = AppointmentToAdd.endDate,
                appointment = AppointmentToAdd.appointment
            };
            ItemDto appointmentDto = appointment.AsDto();

            mockAppointmentBL.Setup(service => service.AddAppointmentAsync(It.IsAny<PostItemDto>())).Returns(Task.FromResult<ItemDto>(appointmentDto));

            var sut = new AppointmentsController(mockAppointmentBL.Object);

            /* Act */
            var res = await sut.AddAppointmentAsync(AppointmentToAdd);

            /* Assert */
            var addedAppointment = (res as CreatedAtActionResult).Value as ItemDto;
            addedAppointment.id.Should().NotBeEmpty();
            addedAppointment.Should().BeEquivalentTo(
                AppointmentToAdd,
                options => options.ComparingByMembers<ItemDto>().ExcludingMissingMembers()
            );
        }

        [Fact]
        public async Task AddAppointmentAsync_withDuplicateAppointmentToAdd_ReturnsConflict()
        {
            /* Arrange */
            PostItemDto AppointmentToAdd = new PostItemDto(new DateTime(2024, 12, 22, 2, 0, 0), new DateTime(2024, 12, 22, 3, 0, 0), "test");

            mockAppointmentBL.Setup(service => service.AddAppointmentAsync(AppointmentToAdd)).Returns(Task.FromResult<ItemDto>(null));

            var sut = new AppointmentsController(mockAppointmentBL.Object);

            /* Act */
            var res = await sut.AddAppointmentAsync(AppointmentToAdd);

            /* Assert */
            res.Should().BeOfType<ConflictObjectResult>();
        }

        [Fact]
        public async Task AddAppointmentAsync_withEqualTimeToAdd_ReturnsBadRequest()
        {
            /* Arrange */
            PostItemDto AppointmentToAdd = new PostItemDto(new DateTime(2022, 12, 22, 8, 0, 0), new DateTime(2022, 12, 22, 8, 0, 0), "test");

            var sut = new AppointmentsController(mockAppointmentBL.Object);

            /* Act */
            var res = await sut.AddAppointmentAsync(AppointmentToAdd);

            /* Assert */
            res.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeleteAppointmentAsync_WithExistingItem_ReturnsNoContent()
        {
            /* Arrange */
            var itemId = Guid.NewGuid();
            mockAppointmentBL.Setup(service => service.DeleteAppointmentAsync(itemId))
               .ReturnsAsync(true);

            var sut = new AppointmentsController(mockAppointmentBL.Object);
            /* Act */
            var res = await sut.DeleteAppointmentAsync(itemId);

            /* Assert */
            res.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteAppointmentAsync_withoutExistingItem_ReturnNotFound()
        {
            /* Arrange */
            var itemId = Guid.NewGuid();
            mockAppointmentBL.Setup(service => service.DeleteAppointmentAsync(itemId))
               .ReturnsAsync(false);

            var sut = new AppointmentsController(mockAppointmentBL.Object);
            /* Act */
            var res = await sut.DeleteAppointmentAsync(itemId);

            /* Assert */
            res.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdateAppointmentAsync_withInvalidTime_ReturnBadRequest()
        {
            /* Arrange */
            var itemId = Guid.NewGuid();
            ItemDto AppointmentToUpdate = new ItemDto(itemId, new DateTime(2022, 12, 22, 8, 0, 0), new DateTime(2022, 12, 22, 8, 0, 0), "test");

            var sut = new AppointmentsController(mockAppointmentBL.Object);
            /* Act */
            var res = await sut.UpdateAppointmentAsync(AppointmentToUpdate);

            /* Assert */
            res.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task UpdateAppointmentAsync_withConflictTime_ReturnConflict()
        {
            /* Arrange */
            var itemId = Guid.NewGuid();
            ItemDto AppointmentToUpdate = new ItemDto(itemId, new DateTime(2024, 12, 22, 8, 0, 0), new DateTime(2024, 12, 22, 9, 0, 0), "test");

            mockAppointmentBL.Setup(service => service.UpdateAppointmentAsync(It.IsAny<ItemDto>()))
               .ReturnsAsync(false);
            var sut = new AppointmentsController(mockAppointmentBL.Object);

            /* Act */
            var res = await sut.UpdateAppointmentAsync(AppointmentToUpdate);

            /* Assert */
            res.Should().BeOfType<ConflictObjectResult>();
        }

        [Fact]
        public async Task UpdateAppointmentAsync_withValidAppointment_ReturnOk()
        {
            /* Arrange */
            var itemId = Guid.NewGuid();
            ItemDto AppointmentToUpdate = new ItemDto(itemId, new DateTime(2024, 12, 22, 8, 0, 0), new DateTime(2024, 12, 22, 9, 0, 0), "test");

            mockAppointmentBL.Setup(service => service.UpdateAppointmentAsync(It.IsAny<ItemDto>()))
               .ReturnsAsync(true);
            var sut = new AppointmentsController(mockAppointmentBL.Object);

            /* Act */
            var res = await sut.UpdateAppointmentAsync(AppointmentToUpdate);

            /* Assert */
            res.Should().BeOfType<OkObjectResult>();
        }
    }
}
