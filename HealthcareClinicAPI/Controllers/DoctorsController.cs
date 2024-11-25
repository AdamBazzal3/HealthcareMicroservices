using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HealthcareClinicAPI.Models;
using HealthcareClinicAPI.Services.DbProxyService;
using System.Net;
using MongoDB.Driver;
using static MongoDB.Driver.WriteConcern;
using Consul.Filtering;
using Polly;
using Polly.CircuitBreaker;
using MongoDB.Bson;

namespace HealthcareClinicAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class DoctorsController : ControllerBase
    {
  
        private readonly ILogger<DoctorsController> _logger;
        private readonly IWebHostEnvironment env;
        private readonly IDbProxy<Doctor> _doctorDbProxy;
        public DoctorsController(IWebHostEnvironment env, ILogger<DoctorsController> logger,IDbProxy<Doctor> doctorsDbProxy)
        {
            this.env = env;
            _logger = logger;
            _doctorDbProxy = doctorsDbProxy;
        }
        #region doctors
        [HttpPost]
        [Route("doctors")]
        public async Task<ActionResult> AddDoctor(Doctor doctor)
        {
            try
            {
                await _doctorDbProxy.CreateAsync(doctor);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("doctors")]
        public async Task<ActionResult> GetDoctors()
        {
            try
            {
                var doctorList = await _doctorDbProxy.GetAsync();

                return Ok(doctorList);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("doctors/available")]
        public async Task<ActionResult> GetAvailableDoctors()
        {
            try
            {
                var filter = Builders<Doctor>.Filter.Eq(d => d.IsAvailable, true);
                var doctorList = await _doctorDbProxy.GetAsync(filter);

                return Ok(doctorList);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("doctors")]
        public async Task<ActionResult> UpdateDoctor(Doctor doctor)
        {
            try
            {
                //await _circuitBreakerPolicy.Execute(async() =>
                //{
                    // Finds the ID of the first restaurant document that matches the filter
                    var filter = Builders<Doctor>.Filter
                    .Eq(d => d._id, doctor._id);
                    //throw new Exception("Simulated exception");
                    await _doctorDbProxy.UpdateAsync(filter, doctor);
                //});
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
        [Route("doctors/{id}")]
        public async Task<ActionResult> DeleteDoctor(string id)
        {
            try
            {
                if(ObjectId.TryParse(id, out ObjectId objId))
                {
                    //await _circuitBreakerPolicy.Execute(async() =>
                    //{
                    // Finds the ID of the first restaurant document that matches the filter
                    var filter = Builders<Doctor>.Filter
                    .Eq(d => d._id, objId);
                    //throw new Exception("Simulated exception");
                    await _doctorDbProxy.RemoveAsync(filter);
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