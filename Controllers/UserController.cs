using Choresbuddy_dotnet.Models;
using Choresbuddy_dotnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Choresbuddy_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return Ok(await _userService.GetUsersAsync());
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST: api/users/register
        [HttpPost("register")]
        public async Task<ActionResult<User>> RegisterUser(string name, string email, string password, string role, int parentId = 0)
        {
            try
            {
                var createdUser = await _userService.RegisterUserAsync(name, email, password, role, parentId);
                return CreatedAtAction(nameof(GetUser), new { userId = createdUser.UserId, role=createdUser.Role }, createdUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginUser(string email, string password)
        {
            try
            {
                var user = await _userService.LoginUserAsync(email, password);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, string name, string email, DateTime dob, string password= "")
        {
            var updated = await _userService.UpdateUserAsync(id, name, email, password, dob);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _userService.DeleteUserAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        // GET: api/users/{parentId}/children
        [HttpGet("{parentId}/children")]
        public async Task<ActionResult<IEnumerable<User>>> GetChildren(int parentId)
        {
            var children = await _userService.GetChildrenAsync(parentId);

            if (children == null || !children.Any())
            {
                return NotFound("No children found for this parent.");
            }

            return Ok(children);
        }

        [HttpGet("child/{childId}/points")]
        public async Task<IActionResult> GetChildPoints(int childId)
        {
            int totalPoints = await _userService.GetUserPointsAsync(childId);
            return Ok(new { childId, points = totalPoints });
        }

        [HttpGet("{userId}/balance")]
        public async Task<IActionResult> GetUserBalance(int userId)
        {
            try
            {
                int balance = await _userService.GetUserBalanceAsync(userId);
                return Ok(new { userId, balance });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
