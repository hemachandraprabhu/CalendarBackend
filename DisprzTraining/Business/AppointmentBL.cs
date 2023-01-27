using DisprzTraining.DataAccess;
using DisprzTraining.Dtos;
using DisprzTraining.Models;

namespace DisprzTraining.Business
{
    public class AppointmentBL : IAppointmentBL
    {
        private readonly IAppointmentDAL _appointmentDAL;
        public AppointmentBL(IAppointmentDAL appointmentDAL)
        {
            _appointmentDAL = appointmentDAL;
        }


        public async Task<List<Appointment>> GetAppointmentsByDateAsync(DateTime date)
        {
            return await _appointmentDAL.GetAppointmentsByDateAsync(date);
        }

        public async Task<List<Appointment>> GetAppointmentsByMonthAsync(DateTime date)
        {
            return await _appointmentDAL.GetAppointmentsByMonthAsync(date);
        }

        public async Task<ItemDto> AddAppointmentAsync(PostItemDto postItemDto)
        {
            Appointment item = new()
            {
                id = Guid.NewGuid(),
                startDate = postItemDto.startDate,
                endDate = postItemDto.endDate,
                appointment = postItemDto.appointment
            };
            var check = await _appointmentDAL.AddAppointmentAsync(item);
            if (check)
            {
                return item.AsDto();
            }
            return null;
        }

        public async Task<bool> UpdateAppointmentAsync(ItemDto putItemDto)
        {
            return await _appointmentDAL.UpdateAppointmentAsync(putItemDto);
        }

        public async Task<bool> DeleteAppointmentAsync(Guid id)
        {
            return await _appointmentDAL.DeleteAppointmentAsync(id);
        }
    }
}


