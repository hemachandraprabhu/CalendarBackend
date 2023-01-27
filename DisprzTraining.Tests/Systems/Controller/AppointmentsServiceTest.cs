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
            DateTime date = new DateTime();
            mockAppointmentBL.Setup(service => service.GetAppointmentsByDateAsync(It.IsAny<DateTime>())).ReturnsAsync(AppointmentsFixture.GetTestAppointments());
            var sut = new AppointmentsController(mockAppointmentBL.Object);

            // When
            var res = await sut.GetAppointmentsByDateAsync(date);

            // Then
            res.Should().BeOfType<OkObjectResult>();
            var okObjectResult = res as OkObjectResult;
            okObjectResult.Value.Should().NotBeNull();
            okObjectResult.Value.Should().BeOfType<List<ItemDto>>();
        }

        [Fact]
        public async Task GetAppointmentByDateAsync_withInValidDate_ReturnsNotFound()
        {
            // Given
            DateTime date = new DateTime();
            mockAppointmentBL.Setup(service => service.GetAppointmentsByDateAsync(It.IsAny<DateTime>())).ReturnsAsync(new List<Appointment>());
            var sut = new AppointmentsController(mockAppointmentBL.Object);

            // When
            var res = await sut.GetAppointmentsByDateAsync(date);

            // Then
            res.Should().BeOfType<NotFoundObjectResult>();
            var notFoundObjectResult = res as NotFoundObjectResult;
            notFoundObjectResult.Value.Should().BeOfType<List<ItemDto>>();
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
            var okObjectResult = res as OkObjectResult;
            okObjectResult.Value.Should().NotBeNull();
            okObjectResult.Value.Should().BeOfType<List<ItemDto>>();
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
            var okObjectResult = res as NotFoundObjectResult;
            okObjectResult.Value.Should().BeOfType<List<ItemDto>>();
        }

        [Fact]
        public async Task AddAppointmentAsync_whenStartTimeIsGreaterThanEndTime_ReturnsBadRequest()
        {
            /* Arrange */
            PostItemDto AppointmentToAdd = new PostItemDto(new DateTime(2023, 1, 28, 8, 0, 0), new DateTime(2023, 1, 28, 7, 0, 0), "test");

            var sut = new AppointmentsController(mockAppointmentBL.Object);

            /* Act */
            var res = await sut.AddAppointmentAsync(AppointmentToAdd);

            /* Assert */
            res.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task AddAppointmentAsync_whenStartTimeIsLessThanCurrentTime_ReturnsBadRequest()
        {
            /* Arrange */
            PostItemDto AppointmentToAdd = new PostItemDto(new DateTime(2023, 1, 1, 8, 0, 0), new DateTime(2023, 1, 1, 9, 0, 0), "test");

            var sut = new AppointmentsController(mockAppointmentBL.Object);

            /* Act */
            var res = await sut.AddAppointmentAsync(AppointmentToAdd);

            /* Assert */
            res.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task AddAppointmentAsync_withValidAppointmentToAdd_ReturnsAddedAppointment()
        {
            /* Arrange */
            PostItemDto AppointmentToAdd = new PostItemDto(new DateTime(2024, 1, 1, 8, 0, 0), new DateTime(2024, 1, 1, 9, 0, 0), "Test");
            ItemDto appointmentDto = new ItemDto(Guid.NewGuid(), AppointmentToAdd.startDate, AppointmentToAdd.endDate, AppointmentToAdd.appointment);

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
        public async Task AddAppointmentAsync_withConflictAppointmentToAdd_ReturnsConflict()
        {
            /* Arrange */
            PostItemDto AppointmentToAdd = new PostItemDto(new DateTime(2023, 1, 28, 9, 0, 0), new DateTime(2024, 1, 28, 11, 0, 0), "test");

            mockAppointmentBL.Setup(service => service.AddAppointmentAsync(AppointmentToAdd)).Returns(Task.FromResult<ItemDto>(null));

            var sut = new AppointmentsController(mockAppointmentBL.Object);

            /* Act */
            var res = await sut.AddAppointmentAsync(AppointmentToAdd);

            /* Assert */
            res.Should().BeOfType<ConflictResult>();
        }

        [Fact]
        public async Task UpdateAppointmentAsync_whenStartTimeIsGreaterThanEndTime_ReturnsBadRequest()
        {
            /* Arrange */
            ItemDto AppointmentToAdd = new ItemDto(Guid.NewGuid(), new DateTime(2023, 1, 28, 8, 0, 0), new DateTime(2023, 1, 28, 7, 0, 0), "test");

            var sut = new AppointmentsController(mockAppointmentBL.Object);

            /* Act */
            var res = await sut.UpdateAppointmentAsync(AppointmentToAdd);

            /* Assert */
            res.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task UpdateAppointmentAsync_whenStartTimeIsLessThanCurrentTime_ReturnsBadRequest()
        {
            /* Arrange */
            ItemDto AppointmentToAdd = new ItemDto(Guid.NewGuid(), new DateTime(2023, 1, 1, 8, 0, 0), new DateTime(2023, 1, 1, 9, 0, 0), "test");

            var sut = new AppointmentsController(mockAppointmentBL.Object);

            /* Act */
            var res = await sut.UpdateAppointmentAsync(AppointmentToAdd);

            /* Assert */
            res.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task UpdateAppointmentAsync_withValidAppointment_ReturnOk()
        {
            /* Arrange */
            ItemDto AppointmentToUpdate = new ItemDto(Guid.NewGuid(), new DateTime(2024, 1, 1, 8, 0, 0), new DateTime(2024, 1, 1, 9, 0, 0), "test");

            mockAppointmentBL.Setup(service => service.UpdateAppointmentAsync(It.IsAny<ItemDto>()))
               .ReturnsAsync(true);
            var sut = new AppointmentsController(mockAppointmentBL.Object);

            /* Act */
            var res = await sut.UpdateAppointmentAsync(AppointmentToUpdate);

            /* Assert */
            res.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task UpdateAppointmentAsync_withConflictTime_ReturnConflict()
        {
            /* Arrange */
            ItemDto AppointmentToUpdate = new ItemDto(Guid.NewGuid(), new DateTime(2023, 1, 28, 9, 0, 0), new DateTime(2024, 1, 28, 11, 0, 0), "test");

            mockAppointmentBL.Setup(service => service.UpdateAppointmentAsync(It.IsAny<ItemDto>()))
               .ReturnsAsync(false);
            var sut = new AppointmentsController(mockAppointmentBL.Object);

            /* Act */
            var res = await sut.UpdateAppointmentAsync(AppointmentToUpdate);

            /* Assert */
            res.Should().BeOfType<ConflictResult>();
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

    }
}
