using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Simple_CRUD_Project.Models;
using Simple_CRUD_Project.Services;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Simple_CRUD_Project.Abstractions;
using Azure.Core;
using System.Net.Mime;

namespace Simple_CRUD_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskTestingController : ControllerBase
    {
        private readonly MainDBContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IFileUpload _fileUpload;

        Regex regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");

        public TaskTestingController(MainDBContext context, IPasswordHasher passwordHasher, IFileUpload fileUpload)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _fileUpload = fileUpload;
        }

        #region Password Hashing 

        [HttpPost("UserRegister")]
        public async Task<IActionResult> UserRegister([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                if (regex.IsMatch(registerRequest.Password))
                {
                    var passwordHash = _passwordHasher.HashPassword(registerRequest.Password);

                    var user = new TblUser
                    {
                        Id = Guid.NewGuid(),
                        Email = registerRequest.Email,
                        Password = passwordHash,
                        CreatedAt = DateTime.Now,
                        IsActive = true
                    };

                    _context.TblUsers.Add(user);
                    await _context.SaveChangesAsync();
                    return Ok("Registration is successful.");
                }
                else
                {
                    return BadRequest("Passwords must be at least 8 characters and contain at" +
                        " 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)");
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [HttpPost("LoginAsync")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var user= await _context.TblUsers.FindAsync(loginRequest.Email);
                if (user != null)
                {
                    var validatePassword = _passwordHasher.VerifyHashPassword(user.Password, loginRequest.Password);

                    if (!validatePassword)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, "Username or password is not correct.");
                    }
                    else
                    {
                        return Ok("Login Successful.");
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "You are not registred.");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Upload Single and Multiple File Upload and Download

        [HttpPost("PostSingleFile")]
        public async Task<IActionResult> PostSingleFile([FromForm] FileUploadModel fileDetails)
        {
            if (fileDetails == null)
            {
                return BadRequest();
            }

            try
            {
                await _fileUpload.PostFileAsync(fileDetails.FileDetails);
                return Ok("Upload Successful.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("PostMultipleFile")]
        public async Task<IActionResult> PostMultipleFile([FromForm] IFormFile[] fileDetails)
        {
            if (fileDetails == null || fileDetails.Length == 0)
            {
                return BadRequest();
            }

            try
            {
                await _fileUpload.PostMultiFileAsync(fileDetails);
                return Ok("Upload Successful.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("DownloadFile/{id}")]
        public async Task<IActionResult> DownloadFile(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            try
            {
                var file = await _fileUpload.GetDownloadFileById(id);

                // Return the file with the appropriate content type
                return File(file.FileData, "application/octet-stream", file.FileName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}