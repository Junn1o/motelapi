using Microsoft.AspNetCore.Mvc;
using motel.Data;
using motel.Models.DTO;
using motel.Repositories;
using System.Globalization;

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
        public IActionResult GetAllPost(
            [FromQuery] string? hireState, 
            [FromQuery] string? statusState, 
            [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice,
            [FromQuery] int? minArea, [FromQuery] int? maxArea,
            [FromQuery] int? category,
            [FromQuery] string? isVip,
            [FromQuery] string? sortBy, [FromQuery] bool isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var postlist = _ipostRepository.GetAllPost(
                hireState, 
                statusState, 
                minPrice, maxPrice, 
                minArea, maxArea,
                category,
                isVip,
                sortBy, isAscending, 
                pageNumber, pageSize);
            if (postlist != null)
            {
                return Ok(postlist);
            }
            else
                return Ok("Data Empty");
        }
        [HttpGet("Get-all-post-admin")]
        public IActionResult GetAllPostAdmin([FromQuery] string? hireState,
            [FromQuery] string? statusState,
            [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice,
            [FromQuery] int? minArea, [FromQuery] int? maxArea,
            [FromQuery] int? category,
            [FromQuery] string? isVip,
            [FromQuery] string? phoneNumb,
            [FromQuery] string? sortBy, [FromQuery] bool isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var postlist = _ipostRepository.GetAllPostAdmin(
                            hireState,
                            statusState,
                            minPrice, maxPrice,
                            minArea, maxArea,
                            category,
                            isVip,
                            phoneNumb,
                            sortBy, isAscending,
                            pageNumber, pageSize);
            if (postlist != null)
            {
                return Ok(postlist);
            }
            else
                return Ok("Data Empty");
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
                return Ok("Data Empty");
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
        [HttpPut("update-basic")]
        public IActionResult UpdatePostM(int id ,[FromBody] UpdatePostManage updatePostBasic)
        {
            var postUpdate = _ipostRepository.UpdatePostM(id, updatePostBasic);
            if (postUpdate != null)
            {
                return Ok(postUpdate);
            }
            else
            {
                return StatusCode(10000);
            }

        }
        [HttpPut("post-approve")]
        public IActionResult post_Approve(int id, [FromBody] Post_Approve post_Approve)
        {
            var postPost = _ipostRepository.post_Approve(id, post_Approve);
            if (postPost != null)
            {
                return Ok(postPost);
            }
            else
            {
                return StatusCode(10000);
            }
        }
        //[HttpPut("update-post-manage")]
        //public IActionResult UpdatePostManage(int id, [FromForm] UpdatePost_Manage updatepost)
        //{
        //    var postUpdate = _ipostRepository.UpdatePostManage(id, updatepost);
        //    if (postUpdate != null)
        //    {
        //        return Ok(postUpdate);
        //    }
        //    else
        //    {
        //        return StatusCode(10000);
        //    }
        //}
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
                return Ok("Room deleted");
            }
        }
    }
}
