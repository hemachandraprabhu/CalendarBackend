using DisprzTraining.Business;
using DisprzTraining.Dtos;
using DisprzTraining.Models;
using Microsoft.AspNetCore.Mvc;

namespace DisprzTraining.Controllers
{
    [Route("api")]
    [ApiController]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentBL _appointmentBL;
        public AppointmentsController(IAppointmentBL appointmentBL)
        {
            _appointmentBL = appointmentBL;
        }

        [HttpGet("appointments/date")]
        [ActionName(nameof(GetAppointmentsByDateAsync))]
        [ProducesResponseType(typeof(List<ItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAppointmentsByDateAsync(DateTime date)
        {
            var item = (await _appointmentBL.GetAppointmentsByDateAsync(date))
                           .Select(item => item.AsDto()).ToList();
            List<ItemDto> notFound = new();
            return item.Any() ? Ok(item) : NotFound(notFound);
        }

        [HttpGet("appointments/month")]
        [ProducesResponseType(typeof(List<ItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAppointmentsByMonthAsync(DateTime month)
        {
            var item = (await _appointmentBL.GetAppointmentsByMonthAsync(month))
                           .Select(item => item.AsDto()).ToList();
            List<ItemDto> notFound = new();
            return item.Any() ? Ok(item) : NotFound(notFound);
        }

        [HttpPost("appointments")]
        [ProducesResponseType(typeof(ItemDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAppointmentAsync(PostItemDto postItemDto)
        {
            if (postItemDto.startDate >= postItemDto.endDate)
            {
                return BadRequest(AppointmentError.ErrorCode_001);
            }
            else if (postItemDto.startDate < DateTime.Now)
            {
                return BadRequest(AppointmentError.ErrorCode_002);
            }

            var res = (await _appointmentBL.AddAppointmentAsync(postItemDto));
            return res != null ? CreatedAtAction(nameof(GetAppointmentsByDateAsync), new { id = res.id }, res) : Conflict();
        }

        [HttpPut("appointments")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateAppointmentAsync(ItemDto putItemDto)
        {
            if (putItemDto.startDate >= putItemDto.endDate)
            {
                return BadRequest(AppointmentError.ErrorCode_001);

            }
            else if (putItemDto.startDate < DateTime.Now)
            {
                return BadRequest(AppointmentError.ErrorCode_002);
            }

            var res = await _appointmentBL.UpdateAppointmentAsync(putItemDto);
            return res ? Ok() : Conflict();
        }

        [HttpDelete("appointments/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAppointmentAsync(Guid id)
        {
            var res = await _appointmentBL.DeleteAppointmentAsync(id);
            return res ? NoContent() : NotFound();
        }

    }
}
