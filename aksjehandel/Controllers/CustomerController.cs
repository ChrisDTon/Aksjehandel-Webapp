using aksjehandel.DAL;
using aksjehandel.Model;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using System.Security.Cryptography;


namespace aksjehandel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _db;
        // Logføring
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomerRepository db, ILogger<CustomerController> logger)
        {
            _db = db;
            _logger = logger;       // Logføring
        }

        public async Task<ActionResult> Get()
        {
            int? userId = HttpContext.Session.GetInt32(Helpers.userId);

            if (userId == null)
            {
                return Unauthorized();
            }

            Customer customer = await _db.Get((int) userId);

            return Ok(customer);
        }

        [HttpGet("logout")]
        public bool Logout()
        {
            // Logout
            _logger.LogInformation("Logout successful");
            HttpContext.Session.Remove(Helpers.userId);
            return true;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginCustomer customer)
        {
            if (ModelState.IsValid)
            {
               int? userId = await _db.Login(customer);
                
                if (userId == null)
                {
                    HttpContext.Session.Remove(Helpers.userId);
                    // Failed logging message
                    _logger.LogInformation("Wrong login information");
                    return BadRequest("Wrong login information");
                }
                
                HttpContext.Session.SetInt32(Helpers.userId, (int) userId);
                 // loggfører registeringen
                _logger.LogInformation("Login successful");
                return Ok("Login successful");
            }
            _logger.LogInformation("Error in input validation");
            return BadRequest("Error in input validation");
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] CreateCustomer customer)
        {
            if (ModelState.IsValid)
            {
                bool returOK = await _db.Register(customer);
                if (!returOK)
                {
                    _logger.LogInformation("Wrong registration information");
                    return BadRequest("Wrong registration information");
                }
                return Ok("Customer successfully registered");
            }
            _logger.LogInformation("Error in input validation");
            return BadRequest("Error in input validation");
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdateCustomer customer)
        {
            int? userId = HttpContext.Session.GetInt32(Helpers.userId);

            if (userId == null)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                bool returnOK = await _db.Update(customer, (int) userId);

                if (!returnOK)
                {
                    _logger.LogInformation("Failed to update profile");
                    return BadRequest("Failed to update profile");
                }

                return Ok("Profile successfully updated");
            }

            _logger.LogInformation("Error in input validation");
            return BadRequest("Error in input validation");
        }

        [HttpPost("password")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePassword input)
        {
            int? userId = HttpContext.Session.GetInt32(Helpers.userId);

            if (userId == null)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                bool returnOK = await _db.ChangePassword(input.password, (int)userId);

                if (!returnOK)
                {
                    _logger.LogInformation("Failed to change password");
                    return BadRequest("Failed to change passworde");
                }

                return Ok("Password changed");
            }

            _logger.LogInformation("Error in input validation");
            return BadRequest("Error in input validation");
        }
    }
}
