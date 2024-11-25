using Microsoft.AspNetCore.Mvc;
using HealthcareIdentityAPI.Data;
using HealthcareIdentityAPI.Models;
using HealthcareIdentityAPI.Utils;

namespace HealthcareIdentityAPI.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly Crypto _cryptoService;
        private readonly DataContext _context;
        public AuthenticationController(ILogger<AuthenticationController> logger, Crypto crypto, DataContext context)
        {
            _logger = logger;
            _cryptoService = crypto;
            _context = context;
        }
        [HttpPost]
        [Route("login")]
        public ActionResult Login(LoginRequestModel userCredentials)
        {
            try
            {
                User user = _context.Users.SingleOrDefault(user => user.UserName.ToLower().Equals(userCredentials.Username));

                if (user != null)
                {
                    if (user.Password.Equals(userCredentials.Password))
                    {
             
                        _logger.LogInformation("user logged in...");
                        return Ok(new LoginResponseModel
                        {
                            Token = _cryptoService.GenerateToken(user),
                            User = user
                        });
                    }
                    else
                        return Unauthorized();
                }
                else
                {
                    return NoContent();
                }
                
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("signup")]
        public ActionResult SignUp(User user)
        {
            try
            {
                user.UserName = user.UserName.ToLower();
                _context.Users.Add(user);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}