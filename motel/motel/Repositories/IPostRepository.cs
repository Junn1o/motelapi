using Microsoft.AspNetCore.Mvc;
using motel.Models.Domain;
using motel.Models.DTO;

namespace motel.Repositories
{
    public interface IPostRepository
    {
        PostListResult GetAllPost(string? filterHire = null, string? hireState = null,
            string? filterStatus = null, string? statusState = null,
            string? filterPrice = null, decimal? minPrice = null, decimal? maxPrice = null,
            string? filterArea = null, int? minArea = null, int? maxArea = null,
            string? filterCate = null, int? category = null,
            string? sortBy = null, bool isAscending = true,
            int pageNumber = 1, int pageSize = 10);
        PostListResult GetAllPostAdmin([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10);
        PostNoIdDTO GetPostByID(int id);
        AddPostDTO AddPost(AddPostDTO addpost);
        AddPostDTO UpdatePost(int id, AddPostDTO updatepost);
        Post DeletePost(int id);
        UpdatePostManage UpdatePostM(int id, UpdatePostManage updatepost);
        Post_Approve post_Approve(int id, Post_Approve post_Approve);
    }
}
