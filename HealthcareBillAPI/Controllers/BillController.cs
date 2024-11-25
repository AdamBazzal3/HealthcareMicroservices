using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HealthcareBillAPI.Models;
using HealthcareBillAPI.Services.DbProxyService;
using System.Net;
using MongoDB.Driver;
using static MongoDB.Driver.WriteConcern;
using Consul.Filtering;
using Polly;
using Polly.CircuitBreaker;
using MongoDB.Bson;

namespace HealthcareBillAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class BillController : ControllerBase
    {
  
        private readonly ILogger<BillController> _logger;
        private readonly IWebHostEnvironment env;
        private readonly IDbProxy<Bill> _BillDbProxy;
        public BillController(IWebHostEnvironment env, 
            ILogger<BillController> logger,
            IDbProxy<Bill> billsDbProxy
        )
        {
            this.env = env;
            _logger = logger;
            _BillDbProxy = billsDbProxy;
        }
        #region records
        [HttpPost]
        [Route("records")]
        public async Task<ActionResult> AddBill(Bill bill)
        {
            try
            {
                await _BillDbProxy.CreateAsync(bill);

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
                var billList = await _BillDbProxy.GetAsync();

                return Ok(billList);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("records")]
        public async Task<ActionResult> UpdateBill(Bill bill)
        {
            try
            {
                //await _circuitBreakerPolicy.Execute(async() =>
                //{
                    // Finds the ID of the first restaurant document that matches the filter
                    var filter = Builders<Bill>.Filter
                    .Eq(p => p._id, bill._id);
                    //throw new Exception("Simulated exception");
                    await _BillDbProxy.UpdateAsync(filter, bill);
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
        [Route("records/{id}")]
        public async Task<ActionResult> DeleteBill(string id)
        {
            try
            {
                if(ObjectId.TryParse(id, out ObjectId objId))
                {
                    //await _circuitBreakerPolicy.Execute(async() =>
                    //{
                    // Finds the ID of the first restaurant document that matches the filter
                    var filter = Builders<Bill>.Filter
                    .Eq(b => b._id, objId);
                    //throw new Exception("Simulated exception");
                    await _BillDbProxy.RemoveAsync(filter);
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