using Microsoft.EntityFrameworkCore;
using motel.Data;
using motel.Models.Domain;
using motel.Models.DTO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace motel.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _appDbContext;
        public PostRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public PostListResult GetAllPost(int pageNumber = 1, int pageSize = 10)
        {
            var postlist = _appDbContext.Post.Select(p => new PostDTO()
            {
                Id = p.Id,
                title = p.title,
                address = p.address,
                authorname = p.user.firstname + " " + p.user.lastname,
                description = p.description,
                price = p.price,
                actualFile = GetImageFromString(p.actualFile),
                area = p.area,
                isHire = p.isHire ? "Đã Được Thuê" : "Chưa Được Thuê",
                status = p.status,
                authorid = p.userId,
                FormattedDatecreated = p.datecreatedroom.ToString("dd/MM/yyyy"),
                categorylist = p.post_category.Select(pc => pc.category.name).ToList(),
            });
            var skipResults = (pageNumber - 1) * pageSize;
            var totalPost = postlist.Count();
            var totalPages = (int)Math.Ceiling((double)totalPost / pageSize);
            var posts = postlist.OrderBy(p=>p.Id).Skip(skipResults).Take(pageSize).ToList();
            var result = new PostListResult
            {
                Post = posts,
                total = totalPost,
                TotalPages = totalPages,
            };
            return result;
        }
        public PostListResult GetAllPostAdmin(int pageNumber = 1, int pageSize = 10)
        {
            var skipResults = (pageNumber - 1) * pageSize;
            var postlist = _appDbContext.Post.Select(p => new PostDTO()
            {
                Id = p.Id,
                title = p.title,
                address = p.address,
                authorname = p.user.firstname + " " + p.user.lastname,
                description = p.description,
                price = p.price,
                actualFile = CountImageFromString(p.actualFile),
                area = p.area,
                isHire = p.isHire ? "Đã Được Thuê" : "Chưa Được Thuê",
                status = p.status,
                authorid = p.userId,
                FormattedDatecreated = p.datecreatedroom.ToString("dd/MM/yyyy"),
                categorylist = p.post_category.Select(pc => pc.category.name).ToList(),
                categoryids = p.post_category.Select(pc => pc.category.Id).ToList(),
            });
            var totalPost = postlist.Count();
            var totalPages = (int)Math.Ceiling((double)totalPost / pageSize);
            var posts = postlist.OrderBy(p => p.Id).Skip(skipResults).Take(pageSize).ToList();
            var result = new PostListResult
            {
                Post = posts,
                total = totalPost,
                TotalPages = totalPages,
            };

            return result;

        }
        public PostNoIdDTO GetPostByID(int id)
        {
            var getPostDomain = _appDbContext.Post.Where(p => p.Id == id);
            var getPostDTO = getPostDomain.Select(p => new PostNoIdDTO()
            {
                title = p.title,
                address = p.address,
                authorname = p.user.firstname + " " + p.user.lastname,
                description = p.description,
                price = p.price,
                actualFile = p.actualFile,
                area = p.area,
                isHire = p.isHire ? "Đã Được Thuê" : "Chưa Được Thuê",
                status = p.status,
                FormattedDatecreated = p.datecreatedroom.ToString("dd/MM/yyyy"),
                categorylist = p.post_category.Select(pc => pc.category.name).ToList(),
            }).FirstOrDefault();
            return getPostDTO;
        }
        public AddPostDTO AddPost(AddPostDTO addpost)
        {
            var user = _appDbContext.User.FirstOrDefault(a => a.Id == addpost.userId);
            if (user.roleId == 1 || user.roleId == 5)
            {
                var postDomain = new Post
                {
                    title = addpost.title,
                    price = addpost.price,
                    address = addpost.address,
                    description = addpost.description,
                    userId = (int)addpost.userId,
                    status = addpost.status = "Đang chờ duyệt",
                    isHire = (bool)(addpost.isHire = false),
                    area = addpost.area,
                    datecreatedroom = (DateTime)(addpost.dateCreated = DateTime.Now),
                };
                _appDbContext.Add(postDomain);
                _appDbContext.SaveChanges();
                //var user = _appDbContext.User.FirstOrDefault(a => a.Id == addpost.userId);
                if (addpost.FileUri != null)
                {
                    addpost.actualFile = UploadImage(addpost.FileUri, user.Id, user.datecreated.ToString("yyyy"), postDomain.Id);
                    postDomain.actualFile = addpost.actualFile;
                    _appDbContext.SaveChanges();
                }
                foreach (var id in addpost.categoryids)
                {
                    var post_category = new Post_Category()
                    {
                        postId = postDomain.Id,
                        categoryId = id,
                    };
                    _appDbContext.Post_Category.Add(post_category);
                    _appDbContext.SaveChanges();
                }
                var postManage = new Post_Manage
                {
                    postId = postDomain.Id,
                };
                _appDbContext.Post_Manage.Add(postManage);
                _appDbContext.SaveChanges();
            }
            else
            {
                return null;
            }
            return addpost;
        }
        public AddPostDTO UpdatePost(int id, AddPostDTO updatepost)
        {
            var postDomain = _appDbContext.Post.FirstOrDefault(r => r.Id == id);
            if (postDomain != null)
            {
                var userDomain = _appDbContext.User.FirstOrDefault(ad => ad.Id == postDomain.userId);
                if (userDomain.roleId == 1 || userDomain.roleId == 5)
                {
                    if (updatepost.FileUri != null)
                    {
                        if (postDomain.actualFile == null || AddNewImagesToPath(postDomain.actualFile, updatepost.FileUri) == null)
                        {
                            updatepost.actualFile = UploadImage(updatepost.FileUri, userDomain.Id, userDomain.datecreated.ToString("yyyy"), postDomain.Id);
                            postDomain.actualFile = updatepost.actualFile;
                            postDomain.FileUri = updatepost.FileUri;
                        }
                        else
                        {
                            updatepost.actualFile = AddNewImagesToPath(postDomain.actualFile, updatepost.FileUri);
                            postDomain.actualFile = updatepost.actualFile;
                            postDomain.FileUri = updatepost.FileUri;
                        }
                    }
                    else
                    {
                        postDomain.actualFile = updatepost.actualFile = postDomain.actualFile;
                    }
                    postDomain.title = updatepost.title;
                    postDomain.description = updatepost.description;
                    postDomain.address = updatepost.address;
                    postDomain.price = updatepost.price;
                    postDomain.status = updatepost.status;
                    postDomain.isHire = (bool)updatepost.isHire;
                    postDomain.area = updatepost.area;
                    _appDbContext.SaveChanges();
                    var categoryrpostDomain = _appDbContext.Post_Category.Where(a => a.postId == id).ToList();
                    if (categoryrpostDomain != null)
                    {
                        _appDbContext.Post_Category.RemoveRange(categoryrpostDomain);
                        _appDbContext.SaveChanges();
                    }
                    foreach (var categoryid in updatepost.categoryids)
                    {
                        var post_category = new Post_Category()
                        {
                            postId = id,
                            categoryId = categoryid,
                        };
                        _appDbContext.Post_Category.Add(post_category);
                        _appDbContext.SaveChanges();
                    }
                    return updatepost;
                }
                else
                    return null;
            }
            else
                return null;
        }
        public UpdatePostManage UpdatePostM(int id, UpdatePostManage updatepost)
        {
            var postDomain = _appDbContext.Post.FirstOrDefault(r => r.Id == id);
            if (postDomain != null)
            {
                postDomain.title = updatepost.title;
                postDomain.description = updatepost.description;
                postDomain.address = updatepost.address;
                postDomain.price = updatepost.price;
                postDomain.status = updatepost.status;
                postDomain.area = updatepost.area;
                if (updatepost.isHire == "Chưa Được Thuê")
                {
                    postDomain.isHire = false;
                }
                else
                {
                    postDomain.isHire = true;
                }
                var categoryrpostDomain = _appDbContext.Post_Category.Where(a => a.postId == id).ToList();
                if (categoryrpostDomain != null)
                {
                    //_appDbContext.Post_Category.RemoveRange(categoryrpostDomain);
                    //_appDbContext.SaveChanges();
                }
                foreach (var categoryid in updatepost.categoryids)
                {
                    var post_category = new Post_Category()
                    {
                        postId = id,
                        categoryId = categoryid,
                    };
                    //_appDbContext.Post_Category.Add(post_category);
                    //_appDbContext.SaveChanges();
                }
                if (updatepost.adminId != null)
                {
                    var postManage = _appDbContext.Post_Manage.FirstOrDefault(pm => pm.postId == id);
                    postManage.userAdminId = updatepost.adminId;
                    postManage.dateapproved = updatepost.dateApprove = DateTime.Now;
                }
                //_appDbContext.SaveChanges();
                return updatepost;
            }
            else
                return null;
        }
        public UpdatePost_Manage UpdatePostManage(int id, UpdatePost_Manage updatepost)
        {
            var postDomain = _appDbContext.Post.FirstOrDefault(r => r.Id == id);
            if (postDomain != null)
            {
                postDomain.title = updatepost.title;
                postDomain.description = updatepost.description;
                postDomain.address = updatepost.address;
                postDomain.price = updatepost.price;
                postDomain.status = updatepost.status;
                postDomain.area = updatepost.area;
                if (updatepost.isHire == "Chưa Được Thuê")
                {
                    postDomain.isHire = false;
                }
                else
                {
                    postDomain.isHire = true;
                }
                var categoryrpostDomain = _appDbContext.Post_Category.Where(a => a.postId == id).ToList();
                if (categoryrpostDomain != null)
                {
                    _appDbContext.Post_Category.RemoveRange(categoryrpostDomain);
                    _appDbContext.SaveChanges();
                }
                foreach (var categoryid in updatepost.categoryids)
                {
                    var post_category = new Post_Category()
                    {
                        postId = id,
                        categoryId = categoryid,
                    };
                    _appDbContext.Post_Category.Add(post_category);
                    _appDbContext.SaveChanges();
                }
                if (updatepost.adminId != null)
                {
                    var postManage = _appDbContext.Post_Manage.FirstOrDefault(pm => pm.postId == id);
                    postManage.userAdminId = updatepost.adminId;
                    postManage.dateapproved = updatepost.dateApprove = DateTime.Now;
                }
                _appDbContext.SaveChanges();
                return updatepost;
            }
            else
                return null;
        }
        public Post DeletePost(int id)
        {
            var postDomain = _appDbContext.Post.FirstOrDefault(r => r.Id == id);
            var postCategory = _appDbContext.Post_Category.Where(n => n.postId == id);
            var postManage = _appDbContext.Post_Manage.Where(n => n.postId == id);
            if (postDomain != null)
            {
                if (postDomain.actualFile != null)
                {
                    DeleteRoomImages(postDomain.actualFile);
                }
                if (postCategory.Any())
                {
                    _appDbContext.Post_Category.RemoveRange(postCategory);
                    _appDbContext.SaveChanges();
                }
                if (postManage.Any())
                {
                    _appDbContext.Post_Manage.RemoveRange(postManage);
                    _appDbContext.SaveChanges();
                }
                _appDbContext.Post.Remove(postDomain);
                _appDbContext.SaveChanges();
            }
            return postDomain;
        }




        public string UploadImage(IFormFile[] file, int id, string adatecreated, int roomid)
        {
            int counter = 1;
            string picture = "";
            foreach (var image in file)
            {
                string count = $"{counter++}";
                var fileEx = Path.GetExtension(image.FileName);
                var uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "user", id + "-" + adatecreated, "uploads", roomid.ToString());
                Directory.CreateDirectory(uploadFolderPath);
                var filePath = Path.Combine(uploadFolderPath, roomid + "-" + "image_" + count + fileEx);
                using (FileStream ms = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(ms);
                }
                string relativePath = Path.Combine("images", "user", id + "-" + adatecreated, "uploads", roomid.ToString(), roomid + "-" + "image_" + count + fileEx);
                picture += relativePath + ";";
            }
            return picture;
        }
        public string AddNewImagesToPath(string imagePath, IFormFile[] newFiles)
        {
            string picture = imagePath;
            string[] existingImagePaths = imagePath.Split(';');
            string[] parts = existingImagePaths[0].Split('\\');
            string idAndDate = parts[2];
            string roomid = parts[4];
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "user", idAndDate, "uploads", roomid);
            if (!File.Exists(existingImagePaths[0]))
            {
                return null;
            }
            else
            {
                int startingCount = existingImagePaths.Length;

                foreach (var image in newFiles)
                {
                    string fileName = roomid + "-image_" + (startingCount++) + Path.GetExtension(image.FileName);

                    var filePath = Path.Combine(folderPath, fileName);
                    using (FileStream ms = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(ms);
                    }
                    string relativePath = Path.Combine("images", "user", idAndDate, "uploads", fileName + Path.GetExtension(image.FileName));
                    picture += relativePath + ";";
                }
                return picture;
            }
        }
        public bool DeleteRoomImages(string imagePath)
        {
            string[] imagePaths = imagePath.Split(';');
            string folderPath = Path.GetDirectoryName(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePaths[0]));
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
                return true;
            }
            else
            {
                return false;
            }
        }
        private static string GetImageFromString(string actualFile)
        {
            if (actualFile == null)
            {
                return null;
            }
            else
            {
                string[] imagePaths = actualFile.Split(';');
                if (imagePaths.Length > 0)
                {
                    return imagePaths[0];
                }
                else
                    return "no image";
            }
        }
        private static string CountImageFromString(string actualFile)
        {
            if (actualFile == null)
            {
                return "0";
            }
            else
            {
                string[] imagePaths = actualFile.TrimEnd(';').Split(';');
                int count = imagePaths.Length;
                if (imagePaths.Length > 0)
                {
                    return count.ToString();
                }
                else
                    return "0";
            }
        }


    }
}
