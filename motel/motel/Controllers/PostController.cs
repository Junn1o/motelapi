using Microsoft.AspNetCore.Mvc;
using motel.Data;
using motel.Models.DTO;
using motel.Repositories;

namespace motel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly IPostRepository _ipostRepository;
        public PostController(AppDbContext appDbContext, IPostRepository ipostRepository)
        {
            _appDbContext = appDbContext;
            _ipostRepository = ipostRepository;
        }
        [HttpGet("Get-all-post")]
        public IActionResult GetAllPost()
        {
            var postlist = _ipostRepository.GetAllPost();
            if (postlist != null)
            {
                return Ok(postlist);
            }
            else
                return NotFound("Data Empty");
        }
        [HttpGet("Get-post-by-id")]
        public IActionResult GetPostbyId(int id)
        {
            var postlist = _ipostRepository.GetPostByID(id);
            if (postlist != null)
            {
                return Ok(postlist);
            }
            else
                return NotFound("Data Empty");
        }
        [HttpPost("add-post")]
        public IActionResult AddPost([FromForm] AddPostDTO addPost)
        {
            var postAdd = _ipostRepository.AddPost(addPost);
            return Ok(postAdd);
        }
        [HttpPut("update-post-by-id")]
        public IActionResult UpdatePost(int id, [FromForm] AddPostDTO updatepost)
        {
            var postUpdate = _ipostRepository.UpdatePost(id, updatepost);
            return Ok(postUpdate);
        }
        [HttpPut("update-post-manage")]
        public IActionResult UpdatePostManage(int id, [FromForm] UpdatePost_Manage updatepost)
        {
            var postUpdate = _ipostRepository.UpdatePostManage(id, updatepost);
            return Ok(postUpdate);
        }
        [HttpDelete("delete-post-with-id")]
        public IActionResult DeletePostwithId(int id)
        {
            var postDelete = _ipostRepository.DeletePost(id);
            if (postDelete == null)
            {
                return StatusCode(500);
            }
            else
            {
                return Ok("author deleted");
            }
        }
    }
}
