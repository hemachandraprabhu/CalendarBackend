using DisprzTraining.Models;

namespace DisprzTraining.Tests.Fixtures
{
    public static class AppointmentsFixture
    {
        public static List<Appointment> GetTestAppointments() => new(){
            new Appointment{
                id = Guid.NewGuid(),
                startDate = new DateTime(2022,12,21,9,0,0),
                endDate = new DateTime(2022,12,21,10,0,0),
                appointment = "abc"
            },
            new Appointment{
                id = Guid.NewGuid(),
                startDate =new DateTime(2022,12,21,9,0,0),
                endDate = new DateTime(2022,12,21,10,0,0),
                appointment = "def"
            },
            new Appointment{
                id = Guid.NewGuid(),
                startDate = new DateTime(2022,12,21,9,0,0),
                endDate = new DateTime(2022,12,21,10,0,0),
                appointment = "ghi"
            }
        };
    }
}