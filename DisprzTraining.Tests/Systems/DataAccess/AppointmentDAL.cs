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
            DateTime d = new DateTime(2022, 12, 21);
            var sut = new AppointmentDAL();

            //Act
            var res = await sut.GetAppointmentsByDateAsync(d);

            //Assert
            // res.Should().BeOfType<List<Appointment>>();
            Assert.IsType<List<Appointment>>(res);
        }

        [Fact]
        public async Task GetppointmentByDateAsync_withInValidDate_ReturnsNull()
        {
            //Arrange
            DateTime d = new DateTime();
            var sut = new AppointmentDAL();

            //Act
            var res = await sut.GetAppointmentsByDateAsync(d);

            //Assert
            // res.Should().BeEmpty();
            Assert.Empty(res);
        }

         [Fact]
        public async Task GetppointmentByMonthAsync_withValidMonth_ReturnsListOfAppointment()
        {
            //Arrange
            DateTime d = new DateTime(2022, 12, 21);
            var sut = new AppointmentDAL();

            //Act
            var res = await sut.GetAppointmentsByMonthAsync(d);

            //Assert
            // res.Should().BeOfType<List<Appointment>>();
            Assert.IsType<List<Appointment>>(res);
        }

        [Fact]
        public async Task GetppointmentByMonthAsync_withInValidMonth_ReturnsNull()
        {
            //Arrange
            DateTime d = new DateTime();
            var sut = new AppointmentDAL();

            //Act
            var res = await sut.GetAppointmentsByMonthAsync(d);

            //Assert
            // res.Should().BeEmpty();
            Assert.Empty(res);
        }

        [Fact]
        public async Task AddAppointmentAsync_withExistingAppointmentToAdd_ReturnsFalse()
        {
            //Arrange
            var existingAppointment = AppointmentDAL.allAppointments[0];
            var sut = new AppointmentDAL();

            //Act
            var res = await sut.AddAppointmentAsync(existingAppointment);

            //Assert
            Assert.Equal(false, res);
        }

        [Fact]
        public async Task AddAppointmentAsync_withNewAppointmentToAdd_ReturnsTrue()
        {
            //Arrange
            Appointment singleAppointment = new Appointment() { id = Guid.NewGuid(), startDate = new DateTime(2023, 12, 21, 9, 0, 0), endDate = new DateTime(2023, 12, 21, 10, 0, 0), appointment = "BLTest" };
            var sut = new AppointmentDAL();

            //Act
            var res = await sut.AddAppointmentAsync(singleAppointment);

            //Assert
            Assert.Equal(true, res);
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
            Assert.Equal(false, res);
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
            Assert.Equal(true, res);
        }

        [Fact]
        public async Task UpdateAppointmentAsync_withoutConflict_ReturnsTrue()
        {
            //Arrange
            ItemDto check = AppointmentDAL.allAppointments[0].AsDto();
            var sut = new AppointmentDAL();

            //Act
            var res = await sut.UpdateAppointmentAsync(check);

            //Assert
            Assert.Equal(true, res);
        }

        [Fact]
        public async Task UpdateAppointmentAsync_withConflict_ReturnsFalse()
        {
            //Arrange
            ItemDto check = new ItemDto(AppointmentDAL.allAppointments[1].id, new DateTime(2023,1,1,6,10,0), new DateTime(2023,1,1,7,30,0), "Town");

            var sut = new AppointmentDAL();

            //Act
            var res = await sut.UpdateAppointmentAsync(check);

            //Assert
            Assert.Equal(false, res);
        }
    }
}