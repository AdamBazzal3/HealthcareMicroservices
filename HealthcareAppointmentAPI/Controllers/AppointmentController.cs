using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MongoDB.Driver;
using static MongoDB.Driver.WriteConcern;
using Consul.Filtering;
using Polly;
using Polly.CircuitBreaker;
using MongoDB.Bson;
using HealthcareAppointmentAPI.Services.DbProxyService;
using HealthcareAppointmentAPI.Models;

namespace HealthcareAppointmentAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class AppointmentController : ControllerBase
    {

        private readonly ILogger<AppointmentController> _logger;
        private readonly IDbProxy<Appointment> _appointmentsRepo;
        private readonly IWebHostEnvironment env;
        public AppointmentController(IWebHostEnvironment env, ILogger<AppointmentController> logger, IDbProxy<Appointment> dbProxy)
        {
            this.env = env;
            _logger = logger;
            _appointmentsRepo = dbProxy;

        }

        #region records
        [HttpPost]
        [Route("records")]
        public async Task<ActionResult> AddRecord(Appointment appointment)
        {
            try
            {
                await _appointmentsRepo.CreateAsync(appointment);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("records")]
        public async Task<ActionResult> GetRecords()
        {
            try
            {
                var appointmentList = await _appointmentsRepo.GetAsync();

                return Ok(appointmentList);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("records")]
        public async Task<ActionResult> UpdateRecord(Appointment appointment)
        {
            try
            {
                
                var filter = Builders<Appointment>.Filter
                .Eq(a => a._id, appointment._id);
                
                await _appointmentsRepo.UpdateAsync(filter, appointment);
                
                return Ok();
            }
            catch (BrokenCircuitException)
            {
                // Return a custom response when the circuit is broken
                return Ok("Service is currently unavailable. Please try again later.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Route("records/{id}")]
        public async Task<ActionResult> DeleteRecord(string id)
        {
            try
            {
                if (ObjectId.TryParse(id, out ObjectId objId))
                {
                    //await _circuitBreakerPolicy.Execute(async() =>
                    //{
                    // Finds the ID of the first restaurant document that matches the filter
                    var filter = Builders<Appointment>.Filter
                    .Eq(p => p._id, objId);
                    //throw new Exception("Simulated exception");
                    await _appointmentsRepo.RemoveAsync(filter);
                    //});
                    return Ok();
                }
                else
                {
                    return BadRequest("invalid id");
                }

            }
            catch (BrokenCircuitException)
            {
                // Return a custom response when the circuit is broken
                return Ok("Service is currently unavailable. Please try again later.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

    }
}