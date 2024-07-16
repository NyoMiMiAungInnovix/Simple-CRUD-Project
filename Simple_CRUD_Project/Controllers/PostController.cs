using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Simple_CRUD_Project.Models;
using System.Net.Http.Headers;

namespace Simple_CRUD_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly MainDBContext _context;

        public PostController(MainDBContext context)
        {
            _context = context;
        }

        #region User Posting

        #region Create

        [HttpPost("CreatePost")]
        public IActionResult CreatePost([FromBody] TblPost post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (post == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                else
                {
                    if (post.UserId == null)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, "UserId field is required.");
                    }
                    if (post.PostContent == null && post.ImageUrl == null)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, "post content or image url is required.");
                    }

                    SaveUserPost(post);
                    return StatusCode(StatusCodes.Status200OK, "Your post is uploaded.");
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        void SaveUserPost(TblPost post)
        {
            post.Id = Guid.NewGuid();
            post.CreatedAt = DateTime.Now;
            post.IsDeleted = false;
            _context.Entry(post).State = EntityState.Added;
            _context.SaveChanges();
        }

        #endregion

        #region Update

        [HttpPut("UpdatePost/{postId}")]
        public IActionResult UpdatePost(string postId, TblPost post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (postId == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "postId should not be null.");
                }

                var postModel = _context.TblPosts.Where(m => m.Id == Guid.Parse(postId)).FirstOrDefault();
                if (postModel == null)
                {
                    return NotFound();
                }

                // Update only non-null values
                if (post.PostContent != null)
                {
                    postModel.PostContent = post.PostContent;
                }
                if (post.ImageUrl != null) 
                { 
                    postModel.ImageUrl = post.ImageUrl;
                }
                postModel.UpdatedAt = DateTime.Now;
                _context.Entry(postModel).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
        }

        #endregion

        #region Delete

        [HttpDelete("DeletePost/{postId}")]
        public IActionResult DeletePost(string postId)
        {
            try
            {
                if (postId == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "postId should not be null.");
                }

                var postModel = _context.TblPosts.Where(post => post.Id == Guid.Parse(postId)).FirstOrDefault();
                if (postModel == null)
                {
                    return NotFound();
                }
                else
                {
                    postModel.IsDeleted = true;
                    _context.Entry(postModel).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                return Ok();
            }
            catch { 
                return NotFound();
            }
        }

        #endregion

        #endregion

        #region Post Comments

        #region Create

        [HttpPost("CreateComment")]
        public IActionResult CreateComment([FromBody] TblComment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (comment == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                else
                {
                    if (comment.PostId == null)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, "PostId field is required.");
                    }
                    if (comment.CommentUserId == null)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, "CommentUserId field is required.");
                    }
                    if (comment.Comment == null)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, "Comment field is required.");
                    }

                    SaveUserComment(comment);
                    return Ok();
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        void SaveUserComment(TblComment comment)
        {
            comment.Id = Guid.NewGuid();
            comment.CreatedAt = DateTime.Now;
            comment.IsDeleted = false;
            _context.Entry(comment).State = EntityState.Added;
            _context.SaveChanges();
        }

        #endregion

        #region Update

        [HttpPut("UpdateComment/{commentId}")]
        public IActionResult UpdateComment(string commentId, TblComment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (commentId == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "commentId should not be null.");
                }
                if (comment.Comment == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Comment field is required.");
                }

                var commentModel = _context.TblComments.Where(m => m.Id == Guid.Parse(commentId)).FirstOrDefault();
                if (commentModel == null)
                {
                    return NotFound();
                }
                commentModel.Comment = comment.Comment;
                commentModel.UpdatedAt = DateTime.Now;
                _context.Entry(commentModel).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
        }

        #endregion

        #region Delete

        [HttpDelete("DeleteComment/{commentId}")]
        public IActionResult DeleteComment(string commentId)
        {
            try
            {
                if (commentId == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "commentId should not be null.");
                }

                var commentModel = _context.TblComments.Where(m => m.Id == Guid.Parse(commentId)).FirstOrDefault();
                if (commentModel == null)
                {
                    return NotFound();
                }
                else
                {
                    commentModel.IsDeleted = true;
                    _context.Entry(commentModel).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }

        #endregion

        #endregion

        #region Get

        [HttpGet("GetPostsByUserId/{userId}")]
        public async Task<ActionResult<IEnumerable<UserPostViewModel>>> GetPostsByUserId(string userId)
        {
            try
            {
                if (userId == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "userId should not be null.");
                }
                var userPosts = await _context.TblUsers.Where(user => user.Id == Guid.Parse(userId))
                                .Select(u => new UserPostViewModel
                                {
                                    UserId = u.Id,
                                    Username = u.Username,
                                    Email = u.Email,
                                    ContactNo = u.ContactNo,
                                    CreatedAt = u.CreatedAt,
                                    IsActive = u.IsActive,
                                    UserPosts = _context.TblPosts.Where(post => post.UserId == u.Id && post.IsDeleted == false).OrderByDescending(post => post.CreatedAt)
                                    .Select(p => new UserPost
                                    {
                                        PostId = p.Id,
                                        PostContent = p.PostContent,
                                        ImageUrl = p.ImageUrl,
                                        CreatedAt = p.CreatedAt,
                                        PostComments = (from comment in _context.TblComments
                                                        join user in _context.TblUsers on comment.CommentUserId equals user.Id
                                                        where comment.PostId == p.Id
                                                        where comment.IsDeleted == false
                                                        orderby comment.CreatedAt ascending
                                                        select new PostComment
                                                        {
                                                            CommentId = comment.Id,
                                                            Comment = comment.Comment,
                                                            CommentUser = user.Username,
                                                            CreatedAt = comment.CreatedAt
                                                        }).ToList()
                                    }).ToList()
                                }).ToListAsync();
                return Ok(userPosts);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #endregion
    }
}
