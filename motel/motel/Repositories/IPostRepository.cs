using Microsoft.AspNetCore.Mvc;
using motel.Models.Domain;
using motel.Models.DTO;

namespace motel.Repositories
{
    public interface IPostRepository
    {
        PostListResult GetAllPost(
            string? hireState = null,
            string? statusState = null,
            decimal? minPrice = null, decimal? maxPrice = null,
            int? minArea = null, int? maxArea = null,
            int? category = null,
            string? isVip = null,
            string? sortBy = null, bool isAscending = true,
            int pageNumber = 1, int pageSize = 10);
        PostListResult GetAllPostAdmin(
            string? hireState = null,
            string? statusState = null,
            decimal? minPrice = null, decimal? maxPrice = null,
            int? minArea = null, int? maxArea = null,
            int? category = null,
            string? isVip = null,
            string? phoneNumb = null,
            string? address = null,
            string? sortBy = null, bool isAscending = true,
            int pageNumber = 1, int pageSize = 10);
        PostNoIdDTO GetPostByID(int id);
        AddPostDTO AddPost(AddPostDTO addpost);
        AddPostDTO UpdatePost(int id, AddPostDTO updatepost);
        Post DeletePost(int id);
        UpdatePostManage UpdatePostM(int id, UpdatePostManage updatepost);
        Post_Approve post_Approve(int id, Post_Approve post_Approve);
        DeleteImg deleteImg(int id, DeleteImg deleteImg);
    }
}
