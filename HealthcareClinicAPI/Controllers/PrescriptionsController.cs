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
    public class PrescriptionsController : ControllerBase
    {
        private readonly ILogger<PrescriptionsController> _logger;
        private readonly IWebHostEnvironment env;
        private readonly IDbProxy<Prescription> _proxy;
        public PrescriptionsController(IWebHostEnvironment env, ILogger<PrescriptionsController> logger,IDbProxy<Doctor> doctorsDbProxy)
        {
            this.env = env;
            _logger = logger;
            _doctorDbProxy = doctorsDbProxy;
        }
        #region prescriptions
        [HttpPost]
        [Route("prescriptions")]
        public async Task<ActionResult> AddPrescription(Prescription p)
        {
            try
            {
                await _proxy.CreateAsync(p);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("prescriptions")]
        public async Task<ActionResult> GetPrescriptions()
        {
            try
            {
                var pList = await _proxy.GetAsync();

                return Ok(pList);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("prescriptions")]
        public async Task<ActionResult> UpdatePrescription(Prescription p)
        {
            try
            {
                //await _circuitBreakerPolicy.Execute(async() =>
                //{
                    // Finds the ID of the first restaurant document that matches the filter
                    var filter = Builders<Prescription>.Filter
                    .Eq(d => d._id, p._id);
                    //throw new Exception("Simulated exception");
                    await _proxy.UpdateAsync(filter, p);
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
        [Route("prescriptions/{id}")]
        public async Task<ActionResult> DeletePrescription(string id)
        {
            try
            {
                if(ObjectId.TryParse(id, out ObjectId objId))
                {
                    //await _circuitBreakerPolicy.Execute(async() =>
                    //{
                    // Finds the ID of the first restaurant document that matches the filter
                    var filter = Builders<Prescription>.Filter
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