using motel.Data;
using motel.Models.Domain;
using motel.Models.DTO;
namespace motel.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _appDbContext;
        public CategoryRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public List<CategoryDTO> GetAllCategory()
        {
            var categorylist = _appDbContext.Category.Select(category=> new CategoryDTO()
            {
                Id = category.Id,
                name = category.name,
            }).ToList();
            return categorylist;
        }
        public CategorywithIdDTO GetCategoryById(int id)
        {
            var getCategorybyDomain = _appDbContext.Category.Where(cd => cd.Id == id);
            var getCategorybyDTO = getCategorybyDomain.Select(category=> new CategorywithIdDTO()
            {
                name = category.name,
                roomlist = category.post_category.Select(cr=>cr.post.title).ToList()
            }).FirstOrDefault();
            return getCategorybyDTO;
        }
        public AddCategoryRequestDTO AddCategory(AddCategoryRequestDTO addCategory)
        {
            var categoryDomain = new Category
            {
                name = addCategory.name,
            };
            _appDbContext.Category.Add(categoryDomain);
            _appDbContext.SaveChanges();
            return addCategory;
        }
        public AddCategoryRequestDTO UpdateCategory(int id, AddCategoryRequestDTO updateCategory)
        {
            var categoryDomain = _appDbContext.Category.FirstOrDefault(cd => cd.Id == id);
            if (categoryDomain != null)
            {
                categoryDomain.name = updateCategory.name;
                _appDbContext.SaveChanges();
            }
            return updateCategory;
        }
        public Category? DeleteCategory(int id)
        {
            var categoryDomain = _appDbContext.Category.FirstOrDefault(c => c.Id == id);
            var categoryRoom = _appDbContext.Post_Category.Where(n => n.postId == id);
            if(categoryDomain != null)
            {
                if(categoryRoom.Any())
                {
                    _appDbContext.Post_Category.RemoveRange(categoryRoom);
                    _appDbContext.SaveChanges();
                }
                _appDbContext.Category.Remove(categoryDomain);
                _appDbContext.SaveChanges();
            }
            return categoryDomain;
        }
    }
}
