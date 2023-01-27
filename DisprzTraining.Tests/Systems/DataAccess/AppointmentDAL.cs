using DisprzTraining.DataAccess;
using DisprzTraining.Dtos;
using DisprzTraining.Models;
using FluentAssertions;

namespace DisprzTraining.Tests.Systems.DataAccess
{

    public class AppointmentDALTest
    {

        [Fact]
        public async Task GetppointmentByDateAsync_withValidDate_ReturnsListOfAppointment()
        {
            //Arrange
            DateTime date = new DateTime(2023, 1, 27);
            var sut = new AppointmentDAL();

            //Act
            var res = await sut.GetAppointmentsByDateAsync(date);

            //Assert
            res.Should().NotBeEmpty();
            res.Should().BeOfType<List<Appointment>>();
            res.Count.Should().Be(1);
        }

        [Fact]
        public async Task GetppointmentByDateAsync_withInValidDate_ReturnsEmptyListOfAppointment()
        {
            //Arrange
            DateTime date = new DateTime(2020, 1, 1);
            var sut = new AppointmentDAL();

            //Act
            var res = await sut.GetAppointmentsByDateAsync(date);

            //Assert
            res.Should().BeEmpty();
            res.Should().BeOfType<List<Appointment>>();
            res.Count.Should().Be(0);
        }

        [Fact]
        public async Task GetppointmentByMonthAsync_withValidMonth_ReturnsListOfAppointment()
        {
            //Arrange
            DateTime date = new DateTime(2023, 1, 27);
            var sut = new AppointmentDAL();

            //Act
            var res = await sut.GetAppointmentsByMonthAsync(date);

            //Assert
            res.Should().NotBeEmpty();
            res.Should().BeOfType<List<Appointment>>();
            res.Count.Should().Be(3);
        }

        [Fact]
        public async Task GetppointmentByMonthAsync_withInValidMonth_ReturnsEmptyListOfAppointment()
        {
            //Arrange
            DateTime date = new DateTime(2020, 1, 1);
            var sut = new AppointmentDAL();

            //Act
            var res = await sut.GetAppointmentsByMonthAsync(date);

            //Assert
            res.Should().BeEmpty();
            res.Should().BeOfType<List<Appointment>>();
            res.Count.Should().Be(0);
        }

        [Fact]
        public async Task AddAppointmentAsync_withoutConflict_ReturnsTrue()
        {
            //Arrange
            Appointment singleAppointment = new Appointment() { id = Guid.NewGuid(), startDate = new DateTime(2023, 12, 30, 9, 0, 0), endDate = new DateTime(2023, 12, 30, 10, 0, 0), appointment = "DALTest" };
            var sut = new AppointmentDAL();

            //Act
            var res = await sut.AddAppointmentAsync(singleAppointment);

            //Assert
            res.Should().BeTrue();

        }

        [Fact]
        public async Task AddAppointmentAsync_withConflictInStartDate_ReturnsFalse()
        {
            //Arrange
            Appointment singleAppointment = new Appointment() { id = Guid.NewGuid(), startDate = new DateTime(2023, 1, 28, 9, 30, 0), endDate = new DateTime(2023, 1, 28, 11, 0, 0), appointment = "Town" };
            var sut = new AppointmentDAL();

            //Act
            var res = await sut.AddAppointmentAsync(singleAppointment);

            //Assert
            res.Should().BeFalse();
        }

        [Fact]
        public async Task AddAppointmentAsync_withConflictInEndDate_ReturnsFalse()
        {
            //Arrange
            Appointment singleAppointment = new Appointment() { id = Guid.NewGuid(), startDate = new DateTime(2023, 1, 28, 8, 0, 0), endDate =  new DateTime(2023, 1, 28, 9, 30, 0), appointment =  "Town" };
            var sut = new AppointmentDAL();

            //Act
            var res = await sut.AddAppointmentAsync(singleAppointment);

            //Assert
            res.Should().BeFalse();
        }

        [Fact]
        public async Task AddAppointmentAsync_withConflictInStartDateEndDate_ReturnsFalse()
        {
            //Arrange
            Appointment singleAppointment = new Appointment() { id = Guid.NewGuid(), startDate =  new DateTime(2023, 1, 28, 8, 0, 0), endDate =  new DateTime(2023, 1, 28, 10, 0, 0), appointment =  "Town" };

            var sut = new AppointmentDAL();

            //Act
            var res = await sut.AddAppointmentAsync(singleAppointment);

            //Assert
            res.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateAppointmentAsync_withoutConflict_ReturnsTrue()
        {
            //Arrange
            ItemDto check = new ItemDto(AppointmentDAL.allAppointments[0].id, new DateTime(2023, 1, 30, 2, 0, 0), new DateTime(2023, 1, 30, 3, 0, 0), "TownHall");
            var sut = new AppointmentDAL();

            //Act
            var res = await sut.UpdateAppointmentAsync(check);

            //Assert
            res.Should().BeTrue();

        }

        [Fact]
        public async Task UpdateAppointmentAsync_withConflictInStartDate_ReturnsFalse()
        {
            //Arrange
            ItemDto check = new ItemDto(AppointmentDAL.allAppointments[0].id, new DateTime(2023, 1, 28, 9, 30, 0), new DateTime(2023, 1, 28, 11, 0, 0), "Town");

            var sut = new AppointmentDAL();

            //Act
            var res = await sut.UpdateAppointmentAsync(check);

            //Assert
            res.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateAppointmentAsync_withConflictInEndDate_ReturnsFalse()
        {
            //Arrange
            ItemDto check = new ItemDto(AppointmentDAL.allAppointments[0].id, new DateTime(2023, 1, 28, 8, 0, 0), new DateTime(2023, 1, 28, 9, 30, 0), "Town");

            var sut = new AppointmentDAL();

            //Act
            var res = await sut.UpdateAppointmentAsync(check);

            //Assert
            res.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateAppointmentAsync_withConflictInStartDateEndDate_ReturnsFalse()
        {
            //Arrange
            ItemDto check = new ItemDto(AppointmentDAL.allAppointments[1].id, new DateTime(2023, 1, 28, 8, 0, 0), new DateTime(2023, 1, 28, 10, 0, 0), "Town");

            var sut = new AppointmentDAL();

            //Act
            var res = await sut.UpdateAppointmentAsync(check);

            //Assert
            res.Should().BeFalse();
        }


        [Fact]
        public async Task DelteAppointmentAsync_withInvalidId_ReturnsFalse()
        {
            //Arrange
            Guid id = Guid.Empty;
            var sut = new AppointmentDAL();

            //Act
            var res = await sut.DeleteAppointmentAsync(id);

            //Assert
            res.Should().BeFalse();
        }

        [Fact]
        public async Task DelteAppointmentAsync_withValidId_ReturnsTrue()
        {
            //Arrange
            var id = AppointmentDAL.allAppointments[0].id;
            var sut = new AppointmentDAL();

            //Act
            var res = await sut.DeleteAppointmentAsync(id);

            //Assert
            res.Should().BeTrue();

        }


    }
}