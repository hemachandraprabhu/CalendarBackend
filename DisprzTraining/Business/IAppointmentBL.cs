using DisprzTraining.Dtos;
using DisprzTraining.Models;

namespace DisprzTraining.Business
{
    public interface IAppointmentBL
    {
        Task<List<Appointment>> GetAppointmentsByDateAsync(DateTime date);
        Task<List<Appointment>> GetAppointmentsByMonthAsync(DateTime date);
        Task<ItemDto> AddAppointmentAsync(PostItemDto postItemDto);
        Task<bool> UpdateAppointmentAsync(ItemDto putItemDto);
        Task<bool> DeleteAppointmentAsync(Guid id);
    }
}