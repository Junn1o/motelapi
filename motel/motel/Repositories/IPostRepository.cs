using Microsoft.AspNetCore.Mvc;
using motel.Models.Domain;
using motel.Models.DTO;

namespace motel.Repositories
{
    public interface IPostRepository
    {
        PostListResult GetAllPost(string? filterOn = null, string? filterQuery = null, string? filterStatus = null, string? filterQuery2 = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 10);
        PostListResult GetAllPostAdmin([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10);
        PostNoIdDTO GetPostByID(int id);
        AddPostDTO AddPost(AddPostDTO addpost);
        AddPostDTO UpdatePost(int id, AddPostDTO updatepost);
        Post DeletePost(int id);
        UpdatePostManage UpdatePostM(int id, UpdatePostManage updatepost);
        Post_Approve post_Approve(int id, Post_Approve post_Approve);
    }
}
