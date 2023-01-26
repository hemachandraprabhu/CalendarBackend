using Moq;
using FluentAssertions;
using DisprzTraining.DataAccess;
using DisprzTraining.Models;
using DisprzTraining.Dtos;
using DisprzTraining.Tests.Fixtures;
using DisprzTraining.Business;

namespace DisprzTraining.Tests.Systems.Buisness
{
    public class AppointmentBLTest
    {
        private readonly Mock<IAppointmentDAL> mockAppointmentDAL = new();
        private Appointment singleAppointment = new Appointment() { id = Guid.NewGuid(), startDate = new DateTime(2022, 12, 21, 9, 0, 0), endDate = new DateTime(2022, 12, 21, 10, 0, 0), appointment = "BLTest" };
        private PostItemDto postItemDto = new PostItemDto(new DateTime(2022, 12, 21, 9, 0, 0), new DateTime(2022, 12, 21, 10, 0, 0), "TownHall");


        [Fact]
        public async Task GetAppointmentByDateAsync_withValidDate_ReturnListOfAppointment()
        {
            // Given
            DateTime d = new DateTime();
            mockAppointmentDAL.Setup(service => service.GetAppointmentsByDateAsync(It.IsAny<DateTime>())).ReturnsAsync(AppointmentsFixture.GetTestAppointments());
            var sut = new AppointmentBL(mockAppointmentDAL.Object);

            // When
            var res = await sut.GetAppointmentsByDateAsync(d);

            // Then
            res.Should().BeOfType<List<Appointment>>();
        }

        [Fact]
        public async Task GetAppointmentByDateAsync_withInValidDate_ReturnNull()
        {
            // Given
            DateTime d = new DateTime();
            mockAppointmentDAL.Setup(service => service.GetAppointmentsByDateAsync(d)).ReturnsAsync(() => null);
            var sut = new AppointmentBL(mockAppointmentDAL.Object);

            // When
            var res = await sut.GetAppointmentsByDateAsync(d);

            // Then
            res.Should().BeNull();
        }
        
        [Fact]
        public async Task GetAppointmentByMonthAsync_withValidMonth_ReturnListOfAppointment()
        {
            // Given
            DateTime d = new DateTime();
            mockAppointmentDAL.Setup(service => service.GetAppointmentsByMonthAsync(It.IsAny<DateTime>())).ReturnsAsync(AppointmentsFixture.GetTestAppointments());
            var sut = new AppointmentBL(mockAppointmentDAL.Object);

            // When
            var res = await sut.GetAppointmentsByMonthAsync(d);

            // Then
            res.Should().BeOfType<List<Appointment>>();
        }

        [Fact]
        public async Task GetAppointmentByMonthAsync_withInValidDate_ReturnNull()
        {
            // Given
            DateTime d = new DateTime();
            mockAppointmentDAL.Setup(service => service.GetAppointmentsByMonthAsync(d)).ReturnsAsync(() => null);
            var sut = new AppointmentBL(mockAppointmentDAL.Object);

            // When
            var res = await sut.GetAppointmentsByMonthAsync(d);

            // Then
            res.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAppointmentAsync_OnFail_returnFalse()
        {
            //Given
            mockAppointmentDAL.Setup(service => service.DeleteAppointmentAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            var sut = new AppointmentBL(mockAppointmentDAL.Object);

            //When
            var res = await sut.DeleteAppointmentAsync(singleAppointment.id);

            //Then
            res.Should().BeFalse();
        }
        [Fact]
        public async Task DeleteAppointmentAsync_OnSucess_returnTrue()
        {
            //Given
            mockAppointmentDAL.Setup(service => service.DeleteAppointmentAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            var sut = new AppointmentBL(mockAppointmentDAL.Object);

            //When
            var res = await sut.DeleteAppointmentAsync(singleAppointment.id);

            //Then
            res.Should().BeTrue();
        }

        [Fact]
        public async Task AddAppointmentAsync_withInvalidAppointment_returnNull()
        {
            // Given
            mockAppointmentDAL.Setup(service => service.AddAppointmentAsync(It.IsAny<Appointment>())).Returns(Task.FromResult<bool>(false));
            var sut = new AppointmentBL(mockAppointmentDAL.Object);

            // When
            var res = await sut.AddAppointmentAsync(postItemDto);

            // Then
            res.Should().BeNull();
        }

        [Fact]
        public async Task AddAppointmentAsync_withValidAppointmentToAdd_returnAddedAppointment()
        {
            // Given
            // ItemDto check = singleAppointment.AsDto();
            mockAppointmentDAL.Setup(service => service.AddAppointmentAsync(It.IsAny<Appointment>())).Returns(Task.FromResult<bool>(true));
            var sut = new AppointmentBL(mockAppointmentDAL.Object);

            // When
            var res = await sut.AddAppointmentAsync(postItemDto);

            // Then
            res.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateAppointmentAsync_withInvalidId_returnFalse()
        {
            //Given
            ItemDto check = singleAppointment.AsDto();
            mockAppointmentDAL.Setup(service => service.UpdateAppointmentAsync(It.IsAny<ItemDto>())).ReturnsAsync(false);
            var sut = new AppointmentBL(mockAppointmentDAL.Object);

            //When
            var res = await sut.UpdateAppointmentAsync(check);

            //Then
            res.Should().BeFalse();
        }
        [Fact]
        public async Task UpdateAppointmentAsync_withValidId_returnTrue()
        {
            //Given
            ItemDto check = singleAppointment.AsDto();
            mockAppointmentDAL.Setup(service => service.UpdateAppointmentAsync(It.IsAny<ItemDto>())).ReturnsAsync(true);
            var sut = new AppointmentBL(mockAppointmentDAL.Object);

            //When
            var res = await sut.UpdateAppointmentAsync(check);

            //Then
            res.Should().BeTrue();
        }
    }
}