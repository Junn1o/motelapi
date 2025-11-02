using motel.Models.Domain;
using motel.Models.DTO;

namespace motel.Repositories
{
    public interface ICategoryRepository
    {
        List<CategoryDTO> GetAllCategory();
        CategorywithIdDTO GetCategoryById(int id);
        AddCategoryRequestDTO AddCategory(AddCategoryRequestDTO addCategory);
        AddCategoryRequestDTO UpdateCategory(int id, AddCategoryRequestDTO updateCategory);
        Category? DeleteCategory(int id);
    }
}
