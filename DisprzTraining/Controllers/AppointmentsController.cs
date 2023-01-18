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
        [ProducesResponseType(typeof(ItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAppointmentsByDateAsync(DateTime date)
        {
            var item = (await _appointmentBL.GetAppointmentsByDateAsync(date))
                           .Select(item => item.AsDto()).ToList();
            List<Appointment> notFound = new();
            return item.Any() ? Ok(item) : NotFound(notFound);
        }

        [HttpGet("appointments/month")]
        [ProducesResponseType(typeof(ItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAppointmentsByMonthAsync(DateTime month)
        {
            var item = (await _appointmentBL.GetAppointmentsByMonthAsync(month))
                           .Select(item => item.AsDto()).ToList();
            List<Appointment> notFound = new();
            return item.Any() ? Ok(item) : NotFound(notFound);
        }



        [HttpPost("appointments")]
        [ProducesResponseType(typeof(ItemDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddAppointmentAsync(PostItemDto postItemDto)
        {
            if ((postItemDto.startDate >= postItemDto.endDate) || (postItemDto.startDate < DateTime.Now))
            {
                if (postItemDto.startDate >= postItemDto.endDate)
                {
                    return BadRequest("Invalid Time");
                }
                if (postItemDto.startDate < DateTime.Now)
                {
                    return BadRequest("cannot create event for past");
                }
            }
            var res = (await _appointmentBL.AddAppointmentAsync(postItemDto));
            return res != null ? CreatedAtAction(nameof(GetAppointmentsByDateAsync), new { id = res.id }, res) : Conflict("event already exist");
        }

        [HttpPut("appointments")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> UpdateAppointmentAsync(ItemDto putItemDto)
        {
            if (putItemDto.startDate >= putItemDto.endDate || (putItemDto.startDate < DateTime.Now))
            {
                if (putItemDto.startDate >= putItemDto.endDate)
                {
                    return BadRequest("Invalid Time");
                }
                if (putItemDto.startDate < DateTime.Now)
                {
                    return BadRequest("cannot update event for past");
                }
            }

            var res = await _appointmentBL.UpdateAppointmentAsync(putItemDto);

            return res ? Ok("event was successfully updated") : Conflict("an event already exist in the choosen time");
        }

        [HttpDelete("appointments/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteAppointmentAsync(Guid id)
        {
            var res = await _appointmentBL.DeleteAppointmentAsync(id);

            return res ? NoContent() : NotFound();
        }



        //design - GET /api/appointments
        //- POST /api/appointments
        //- DELETE /api/appointments

        //refer hello world controller for BL & DAL logic 

        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Appointment))]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> GerAppointments()
        //{
        //    return Ok();
        //}

    }
}
