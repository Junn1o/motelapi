using Microsoft.AspNetCore.Mvc;
using motel.Models.Domain;
using motel.Models.DTO;

namespace motel.Repositories
{
    public interface IPostRepository
    {
        PostListResult GetAllPost([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10);
        PostListResult GetAllPostAdmin([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10);
        PostNoIdDTO GetPostByID(int id);
        AddPostDTO AddPost(AddPostDTO addpost);
        AddPostDTO UpdatePost(int id, AddPostDTO updatepost);
        UpdatePost_Manage UpdatePostManage(int id, UpdatePost_Manage updatePost_Manage);
        Post DeletePost(int id);
        UpdatePostManage UpdatePostM(int id, UpdatePostManage updatePostBasic);
    }
}
