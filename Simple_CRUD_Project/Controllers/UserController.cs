﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Simple_CRUD_Project.Abstractions;
using Simple_CRUD_Project.Models;
using System.Net.Http.Headers;

namespace Simple_CRUD_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MainDBContext _context;

        public UserController(MainDBContext context)
        {
            _context = context;
        }

        #region Create

        [HttpPost("CreateUser")]
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
            user.IsActive = true;
            _context.Entry(user).State = EntityState.Added;
            _context.SaveChanges();
        }

        #endregion

        #region Update

        [HttpPut("UpdateUser/{userId}")]
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
                    _context.Update(userModel);
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
        [HttpGet("GetUsers")]
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
