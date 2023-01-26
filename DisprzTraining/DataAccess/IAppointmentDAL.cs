using DisprzTraining.Dtos;
using DisprzTraining.Models;

namespace DisprzTraining.DataAccess
{
    public interface IAppointmentDAL
    {
        Task<List<Appointment>> GetAppointmentsByDateAsync(DateTime date);
        Task<List<Appointment>> GetAppointmentsByMonthAsync(DateTime date);
        Task<bool> AddAppointmentAsync(Appointment appointment);
        Task<bool> UpdateAppointmentAsync(ItemDto putItemDto);
        Task<bool> DeleteAppointmentAsync(Guid id);
    }
}