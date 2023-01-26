using DisprzTraining.Dtos;
using DisprzTraining.Models;

namespace DisprzTraining
{
    public static class Extensions
    {
        public static ItemDto AsDto(this Appointment item)
        {
            return new ItemDto(item.id, item.startDate, item.endDate, item.appointment);
        }
    }
}