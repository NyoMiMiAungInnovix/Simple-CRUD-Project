using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Simple_CRUD_Project.Models;
using System.Net.Http.Headers;

namespace Simple_CRUD_Project.Controllers
{
    [Produces("application/json")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        MainDBContext _context = new MainDBContext();
        private HttpClient client;

        public UserController()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #region Create

        [HttpPost]
        public IActionResult CreateUser([FromBody] TblUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                else
                {
                    if (user.Username == null)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, "Username field is required.");
                    }
                    if (user.Email == null)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, "Email field is required.");
                    }
                    if (user.ContactNo == null)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, "ContactNo field is required.");
                    }

                    SaveUserData(user);
                    return Ok("user creation is successful.");
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        void SaveUserData(TblUser user)
        {
            user.Id = Guid.NewGuid();
            user.CreatedAt = DateTime.Now;
            user.IsActive = true;
            _context.Entry(user).State = EntityState.Added;
            _context.SaveChanges();
        }

        #endregion

        #region Update

        [HttpPut("{userId}")]
        public IActionResult UpdateUser(string userId, [FromBody] TblUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                else
                {
                    if (userId == null)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, "userId should not be null.");
                    }

                    var userModel = _context.TblUsers.Where(user => user.Id == Guid.Parse(userId) && user.IsActive == true).FirstOrDefault();
                    if (userModel == null)
                    {
                        return NotFound();
                    }

                    #region Update User Information

                    // Update only non-null values
                    if (user.Username != null)
                    {
                        userModel.Username = user.Username;
                    }
                    if (user.Email != null)
                    {
                        userModel.Email = user.Email;
                    }
                    if (user.ContactNo != null)
                    {
                        userModel.ContactNo = user.ContactNo;
                    }
                    if (user.IsActive != null)
                    {
                        userModel.IsActive = user.IsActive;
                    }
                    userModel.UpdatedAt = DateTime.Now;
                    _context.Entry(userModel).State = EntityState.Modified;
                    _context.SaveChanges();

                    #endregion                  

                    return Ok("Update user informaion is successful.");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
        }

        #endregion

        #region Get

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblUser>>> GetUsers()
        {
            try
            {
                return await _context.TblUsers.Where(user => user.IsActive == true).OrderByDescending(user => user.CreatedAt).ToListAsync();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #endregion

    }
}
