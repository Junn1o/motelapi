using motel.Models.Domain;
using motel.Models.DTO;

namespace motel.Repositories
{
    public interface IPostRepository
    {
        List<PostDTO> GetAllPost();
        PostNoIdDTO GetPostByID(int id);
        AddPostDTO AddPost(AddPostDTO addpost);
        AddPostDTO UpdatePost(int id, AddPostDTO updatepost);
        Post DeletePost(int id);
    }
}
