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
    public class MedicalRecordsController : ControllerBase
    {
  
        private readonly ILogger<MedicalRecordsController> _logger;
        private readonly IWebHostEnvironment env;
        private readonly IDbProxy<MedicalRecord> _medicalRecordProxy;
        Random rnd = new Random();
        private static DateTime _recoveryTime  =DateTime.UtcNow;

        public MedicalRecordsController(IWebHostEnvironment env, ILogger<MedicalRecordsController> logger,IDbProxy<MedicalRecord> medicalRecorddbProxy)
        {
            this.env = env;
            _logger = logger;
            _medicalRecordProxy = medicalRecorddbProxy;
        }
        #region medicalRecords
        [HttpPost]
        [Route("medicalRecords")]
        public async Task<ActionResult> AddRecord(MedicalRecord record)
        {
            try
            {
                await _medicalRecordProxy.CreateAsync(record);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("medicalRecords")]
        public async Task<ActionResult> GetMedicalRecords()
        {
            try
            {
                var records = await _medicalRecordProxy.GetAsync();

                return Ok(records);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("medicalRecords/{patientId}")]
        public async Task<ActionResult> GetMedicalRecords(string patientId)
        {
            try
            {
                // Check if the service is currently unavailable
                if (_recoveryTime > DateTime.UtcNow)
                {
                    throw new Exception("Clinic Service is not available now.");
                }

                // Randomly set a new recovery time to simulate a temporary failure
                if (rnd.Next(1, 4) == 1)
                {
                    _recoveryTime = DateTime.UtcNow.AddSeconds(25);
                }

                var filter = Builders<MedicalRecord>.Filter
                    .Eq(d => d.Patient.Id, patientId);
                var records = await _medicalRecordProxy.GetAsync(filter);

                return Ok(records);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("medicalRecords")]
        public async Task<ActionResult> UpdateRecord(MedicalRecord medicalRecord)
        {
            try
            {
                //await _circuitBreakerPolicy.Execute(async() =>
                //{
                    // Finds the ID of the first restaurant document that matches the filter
                    var filter = Builders<MedicalRecord>.Filter
                    .Eq(d => d._id, medicalRecord._id);
                    //throw new Exception("Simulated exception");
                    await _medicalRecordProxy.UpdateAsync(filter, medicalRecord);
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
        [Route("medicalRecords/{id}")]
        public async Task<ActionResult> DeleteRecord(string id)
        {
            try
            {
                if(ObjectId.TryParse(id, out ObjectId objId))
                {
                    //await _circuitBreakerPolicy.Execute(async() =>
                    //{
                    // Finds the ID of the first restaurant document that matches the filter
                    var filter = Builders<MedicalRecord>.Filter
                    .Eq(d => d._id, objId);
                    //throw new Exception("Simulated exception");
                    await _medicalRecordProxy.RemoveAsync(filter);
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