using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using motel.Data;
using motel.Models.Domain;
using motel.Models.DTO;

namespace motel.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _appDbContext;
        public PostRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public PostListResult GetAllPost(
            string? hireState = null,
            string? statusState = null,
            decimal? minPrice = null, decimal? maxPrice = null,
            int? minArea = null, int? maxArea = null,
            int? category = null,
            string? isVip = null,
            string? sortBy = null, bool isAscending = true,
            int pageNumber = 1, int pageSize = 10)
        {
            DateTime now = DateTime.Now;
            DateTime oneDayAgo = now.AddDays(-30);
            var postlist = _appDbContext.Post
                .Include(pm => pm.post_manage)
                .Include(pm => pm.post_category)
                .ThenInclude(pm => pm.category)
                .Include(pm => pm.user)
                .ThenInclude(pm => pm.users_tier)
                .ThenInclude(pm => pm.tiers)
                .Select(p => new PostDTO()
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
                    expireDate = p.datecreatedroom.AddDays(30).ToString("dd/MM/yyyy"),
                    dateCreated = p.datecreatedroom,
                    dateApproved = p.post_manage.dateapproved.HasValue ? p.post_manage.dateapproved.Value : new DateTime(2020, 10, 22),
                    FormattedDatecreated = p.datecreatedroom.ToString("dd/MM/yyyy"),
                    FormattedDateapprove = p.post_manage.dateapproved != null ? p.post_manage.dateapproved.Value.ToString("dd/MM/yyyy") : "Chưa Có Ngày Duyệt",
                    postTier = p.user.users_tier.tiers.tiername,
                    phone = p.user.phone,
                    categoryids = p.post_category.Select(pc => pc.category.Id).ToList(),
                    categorylist = p.post_category.Select(pc => pc.category.name).ToList(),
                }).AsSplitQuery();

            // Search theo trạng thái thuê
            if (!string.IsNullOrWhiteSpace(hireState))
            {
                postlist = postlist.Where(x => x.isHire.Contains(hireState));
            }
            // Search theo trạng thái duyệt
            if (!string.IsNullOrWhiteSpace(statusState))
            {
                postlist = postlist.Where(x => x.status.Contains(statusState));
            }

            // Search theo range price hoặc fixed price
            if (minPrice.HasValue && maxPrice.HasValue)
            {
                postlist = postlist.Where(x => x.price >= minPrice && x.price <= maxPrice);
            }
            else if (minPrice.HasValue)
            {
                postlist = postlist.Where(x => x.price.Equals(minPrice));
            }
            else if (maxPrice.HasValue)
            {
                postlist = postlist.Where(x => x.price.Equals(maxPrice));
            }

            // Search theo area range hoặc fixed range
            if (minArea.HasValue && maxArea.HasValue)
            {
                postlist = postlist.Where(x => x.area >= minArea && x.area <= maxArea);
            }
            else if (minArea.HasValue)
            {
                postlist = postlist.Where(x => x.area.Equals(minArea));
            }
            else if (maxArea.HasValue)
            {
                postlist = postlist.Where(x => x.area.Equals(maxArea));
            }

            // Search theo area range
            if (category.HasValue)
            {
                postlist = postlist.Where(x => x.categoryids.Contains((int)category));
            }

            //List theo tin vip hay thường
            if (!string.IsNullOrWhiteSpace(isVip))
            {
                if (isVip.Equals("Hạng Thường", StringComparison.OrdinalIgnoreCase))
                {
                    postlist = postlist.Where(x => x.postTier.Equals("Hạng Thường"));
                }
                else if (isVip.Equals("Hạng Vip", StringComparison.OrdinalIgnoreCase))
                {
                    postlist = postlist.Where(x => x.postTier.Equals("Hạng Vip"));
                }
            }
            if (postlist == null)
            {
                return null;
            }
            var skipResults = (pageNumber - 1) * pageSize;
            // Sort theo ngày tạo gần nhất
            if (!string.IsNullOrWhiteSpace(sortBy) && sortBy.Equals("dateCreated", StringComparison.OrdinalIgnoreCase))
            {
                var posts = isAscending ?
                    postlist
                    .OrderBy(x => x.dateCreated)
                    .Skip(skipResults)
                    .Take(pageSize)
                    .ToList() :
                    postlist
                    .OrderByDescending(x => x.dateCreated)
                    .Skip(skipResults)
                    .Take(pageSize)
                    .ToList();
                if (posts == null || !posts.Any())
                {
                    return null;
                }
                var totalPost = postlist.Count();
                var totalPages = (int)Math.Ceiling((double)totalPost / pageSize);
                var result = new PostListResult
                {
                    Post = posts,
                    total = totalPost,
                    TotalPages = totalPages,
                };
                return result;
            }
            if (!string.IsNullOrWhiteSpace(sortBy) && sortBy.Equals("newList", StringComparison.OrdinalIgnoreCase))
            {
                var posts = isAscending ?
                    postlist.Where(p => p.dateCreated <= now && p.dateCreated >= oneDayAgo)
                        .OrderBy(x => x.dateCreated)
                        .Skip(skipResults)
                        .Take(pageSize)
                        .ToList() :
                    postlist.Where(p => p.dateCreated <= now && p.dateCreated >= oneDayAgo)
                        .OrderByDescending(x => x.dateCreated)
                        .Skip(skipResults)
                        .Take(pageSize)
                        .ToList();
                if (posts == null || !posts.Any())
                {
                    return null;
                }
                var totalPost = posts.Count();
                var totalPages = (int)Math.Ceiling((double)totalPost / pageSize);
                var result = new PostListResult
                {
                    Post = posts,
                    total = totalPost,
                    TotalPages = totalPages,
                };
                return result;
            }
            else
            {
                var posts = postlist
                    .OrderBy(p => p.Id)
                    .Skip(skipResults)
                    .Take(pageSize)
                    .ToList();
                if (posts == null || !posts.Any())
                {
                    return null;
                }
                var totalPost = postlist.Count();
                var totalPages = (int)Math.Ceiling((double)totalPost / pageSize);
                var result = new PostListResult
                {
                    Post = posts,
                    total = totalPost,
                    TotalPages = totalPages,
                };
                return result;
            }
        }
        public PostListResult GetAllPostAdmin(
            string? hireState = null,
            string? statusState = null,
            decimal? minPrice = null, decimal? maxPrice = null,
            int? minArea = null, int? maxArea = null,
            int? category = null,
            string? isVip = null,
            string? phoneNumb = null,
            string? address = null,
            string? sortBy = null, bool isAscending = true,
            int pageNumber = 1, int pageSize = 10)
        {
            DateTime now = DateTime.Now;
            DateTime oneDayAgo = now.AddDays(-30);
            var postlist = _appDbContext.Post.Include(pm => pm.post_manage)
                .Include(pm => pm.post_category)
                .ThenInclude(pm => pm.category)
                .Include(pm => pm.user)
                .ThenInclude(pm => pm.users_tier)
                .ThenInclude(pm => pm.tiers)
                .Select(p => new PostDTO()
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
                    postTier = p.user.users_tier.tiers.tiername,
                    status = p.status,
                    authorid = p.userId,
                    phone = p.user.phone,
                    expireDate = p.datecreatedroom.AddDays(30).ToString("dd/MM/yyyy"),
                    dateCreated = p.datecreatedroom,
                    reason = p.post_manage.reason != null ? p.post_manage.reason.ToString() : "Không Bị Từ Chối Duyệt",
                    FormattedDatecreated = p.datecreatedroom.ToString("dd/MM/yyyy"),
                    FormattedDateapprove = p.post_manage.dateapproved != null ? p.post_manage.dateapproved.Value.ToString("dd/MM/yyyy") : "Chưa Có Ngày Duyệt",
                    categorylist = p.post_category.Select(pc => pc.category.name).ToList(),
                    categoryids = p.post_category.Select(pc => pc.category.Id).ToList(),
                }).AsSplitQuery();
            if (!string.IsNullOrWhiteSpace(hireState))
            {
                postlist = postlist.Where(x => x.isHire.Contains(hireState));
            }
            // Search theo trạng thái duyệt
            if (!string.IsNullOrWhiteSpace(statusState))
            {
                postlist = postlist.Where(x => x.status.Contains(statusState));
            }

            // Search theo range price hoặc fixed price
            if (minPrice.HasValue && maxPrice.HasValue)
            {
                postlist = postlist.Where(x => x.price >= minPrice && x.price <= maxPrice);
            }
            else if (minPrice.HasValue)
            {
                postlist = postlist.Where(x => x.price.Equals(minPrice));
            }
            else if (maxPrice.HasValue)
            {
                postlist = postlist.Where(x => x.price.Equals(maxPrice));
            }

            // Search theo area range hoặc fixed range
            if (minArea.HasValue && maxArea.HasValue)
            {
                postlist = postlist.Where(x => x.area >= minArea && x.area <= maxArea);
            }
            else if (minArea.HasValue)
            {
                postlist = postlist.Where(x => x.area.Equals(minArea));
            }
            else if (maxArea.HasValue)
            {
                postlist = postlist.Where(x => x.area.Equals(maxArea));
            }

            // Search theo area range
            if (category.HasValue)
            {
                postlist = postlist.Where(x => x.categoryids.Contains((int)category));
            }

            // Search theo sdt
            if (!string.IsNullOrWhiteSpace(phoneNumb))
            {
                postlist = postlist.Where(x => x.phone.Contains(phoneNumb));
            }

            // Search theo address
            if (!string.IsNullOrWhiteSpace(address))
            {
                postlist = postlist.Where(x => x.address.Contains(address));
            }

            //List theo tin vip hay thường
            if (!string.IsNullOrWhiteSpace(isVip))
            {
                if (isVip.Equals("Hạng Thường", StringComparison.OrdinalIgnoreCase))
                {
                    postlist = postlist.Where(x => x.postTier.Equals("Hạng Thường"));
                }
                else if (isVip.Equals("Hạng Vip", StringComparison.OrdinalIgnoreCase))
                {
                    postlist = postlist.Where(x => x.postTier.Equals("Hạng Vip"));
                }
            }
            if (postlist == null)
            {
                return null;
            }
            var skipResults = (pageNumber - 1) * pageSize;
            // Sort theo ngày tạo gần nhất
            if (!string.IsNullOrWhiteSpace(sortBy) && sortBy.Equals("dateCreated", StringComparison.OrdinalIgnoreCase))
            {
                var posts = isAscending ?
                    postlist
                    .OrderBy(x => x.dateCreated)
                    .Skip(skipResults)
                    .Take(pageSize)
                    .ToList() :
                    postlist
                    .OrderByDescending(x => x.dateCreated)
                    .Skip(skipResults)
                    .Take(pageSize)
                    .ToList();
                if (posts == null || !posts.Any())
                {
                    return null;
                }
                var totalPost = postlist.Count();
                var totalPages = (int)Math.Ceiling((double)totalPost / pageSize);
                var result = new PostListResult
                {
                    Post = posts,
                    total = totalPost,
                    TotalPages = totalPages,
                };
                return result;
            }
            if (!string.IsNullOrWhiteSpace(sortBy) && sortBy.Equals("newList", StringComparison.OrdinalIgnoreCase))
            {
                var posts = isAscending ?
                    postlist.Where(p => p.dateCreated <= now && p.dateCreated >= oneDayAgo)
                        .OrderBy(x => x.dateCreated)
                        .Skip(skipResults)
                        .Take(pageSize)
                        .ToList() :
                    postlist.Where(p => p.dateCreated <= now && p.dateCreated >= oneDayAgo)
                        .OrderByDescending(x => x.dateCreated)
                        .Skip(skipResults)
                        .Take(pageSize)
                        .ToList();
                if (posts == null || !posts.Any())
                {
                    return null;
                }
                var totalPost = posts.Count();
                var totalPages = (int)Math.Ceiling((double)totalPost / pageSize);
                var result = new PostListResult
                {
                    Post = posts,
                    total = totalPost,
                    TotalPages = totalPages,
                };
                return result;
            }
            else
            {
                var posts = postlist
                    .OrderBy(p => p.Id)
                    .Skip(skipResults)
                    .Take(pageSize)
                    .ToList();
                if (posts == null || !posts.Any())
                {
                    return null;
                }
                var totalPost = postlist.Count();
                var totalPages = (int)Math.Ceiling((double)totalPost / pageSize);
                var result = new PostListResult
                {
                    Post = posts,
                    total = totalPost,
                    TotalPages = totalPages,
                };
                return result;
            }
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
                FormattedDateapprove = p.post_manage.dateapproved != null ? p.post_manage.dateapproved.Value.ToString("dd/MM/yyyy") : "Chưa Có Ngày Duyệt",
                categorylist = p.post_category.Select(pc => pc.category.name).ToList(),
            }).FirstOrDefault();
            return getPostDTO;
        }
        public AddPostDTO AddPost(AddPostDTO addpost)
        {
            var user = _appDbContext.User.FirstOrDefault(a => a.Id == addpost.userId);
            var postDomain = new Post
            {
                title = addpost.title,
                price = addpost.price,
                address = addpost.address,
                description = addpost.description,
                userId = (int)addpost.userId,
                status = addpost.status = "Đang Chờ Duyệt",
                isHire = (bool)(addpost.isHire = false),
                area = addpost.area,
                datecreatedroom = (DateTime)(addpost.dateCreated = DateTime.Now),
            };
            _appDbContext.Add(postDomain);
            _appDbContext.SaveChanges();
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
            return addpost;
        }
        public AddPostDTO UpdatePost(int id, AddPostDTO updatepost)
        {
            var postDomain = _appDbContext.Post.FirstOrDefault(r => r.Id == id);
            if (postDomain != null)
            {
                var userDomain = _appDbContext.User.FirstOrDefault(ad => ad.Id == postDomain.userId);
                if (CheckFileExists(postDomain.actualFile) != null)
                {
                    postDomain.actualFile = CheckFileExists(postDomain.actualFile);
                }
                if (updatepost.FileUri != null)
                {
                    if (postDomain.actualFile == null || postDomain.actualFile == "")
                    {
                        updatepost.actualFile = UploadImage(updatepost.FileUri, userDomain.Id, userDomain.datecreated.ToString("yyyy"), postDomain.Id);
                        postDomain.FileUri = updatepost.FileUri;
                        postDomain.actualFile = updatepost.actualFile;
                    }
                    else
                    {
                        updatepost.actualFile = AddNewImagesToPath(postDomain.actualFile, updatepost.FileUri);
                        postDomain.FileUri = updatepost.FileUri;
                        postDomain.actualFile = updatepost.actualFile;
                    }
                }
                else
                {
                    updatepost.actualFile = postDomain.actualFile;
                }
                if(updatepost.status != "Đã Duyệt")
                {
                    if (updatepost.isHire != true)
                        postDomain.datecreatedroom = DateTime.Now;
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
                }
                return updatepost;
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
                _appDbContext.SaveChanges();
                return updatepost;
            }
            else
                return null;
        }
        public Post_Approve post_Approve(int id, Post_Approve post_Approve)
        {
            var postDomain = _appDbContext.Post.FirstOrDefault(r => r.Id == id);
            if (postDomain != null)
            {
                // Đang Chờ Duyệt,Không Chấp Nhận Duyệt, Đã Duyệt, Đã Ẩn
                var postManage = _appDbContext.Post_Manage.FirstOrDefault(pm => pm.postId == id);
                if (postManage == null && postDomain != null)
                {

                    var postM = new Post_Manage
                    {
                        postId = postDomain.Id,
                    };
                    _appDbContext.Post_Manage.Add(postM);
                    _appDbContext.SaveChanges();
                }
                if (post_Approve.status.ToString() == "Không Chấp Nhận Duyệt")
                {
                    postManage.reason = post_Approve.reason;
                    postManage.dateapproved = null;
                    postManage.userAdminId = null;
                    postDomain.status = post_Approve.status;
                }
                if (post_Approve.status.ToString() == "Đã Duyệt")
                {
                    postManage.userAdminId = post_Approve.userAdminId;
                    postManage.dateapproved = post_Approve.dateApproved = DateTime.Now;
                    postDomain.status = post_Approve.status;
                    postManage.reason = null;
                }
                if (post_Approve.status.ToString() == "Đã Ẩn")
                {
                    postDomain.status = post_Approve.status;
                    postManage.dateapproved = null;
                    postManage.userAdminId = null;
                    postManage.reason = post_Approve.reason;
                }
                if (post_Approve.status.ToString() == "Đang Chờ Duyệt")
                {
                    postDomain.status = post_Approve.status;
                    postManage.dateapproved = null;
                    postManage.userAdminId = null;
                    postManage.reason = null;
                }
                _appDbContext.SaveChanges();
            }
            return post_Approve;
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
        public DeleteImg deleteImg(int id, DeleteImg deleteImg)
        {
            var postDomain = _appDbContext.Post.FirstOrDefault(r => r.Id == id);
            if (postDomain != null)
            {
                string[] filePaths = deleteImg.actualFile.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                string existingImages = "";
                if (deleteImg.actualFile != null)
                {
                    foreach (string image in filePaths)
                    {
                        string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image);
                        if (File.Exists(folderPath))
                        {
                            File.Delete(folderPath);
                            postDomain.actualFile = postDomain.actualFile.Replace(image + ";", string.Empty);
                        }
                        else
                        {
                            postDomain.actualFile = postDomain.actualFile.Replace(image + ";", string.Empty);
                        }
                    }
                    if (postDomain.actualFile.EndsWith(";"))
                    {
                        postDomain.actualFile = postDomain.actualFile.TrimEnd(';');
                    }
                }
                _appDbContext.SaveChanges();
            }
            return null;
        }
        public string AddNewImagesToPath(string imagePath, IFormFile[] newFiles)
        {
            string picture = imagePath;
            string[] existingImagePaths = imagePath.Split(';');
            string[] parts = existingImagePaths[0].Split('\\');
            string idAndDate = parts[2];
            string roomid = parts[4];
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "user", idAndDate, "uploads", roomid.ToString());
            int startingCount = existingImagePaths.Length;
            foreach (var image in newFiles)
            {
                string fileName = roomid + "-image_" + (startingCount++) + Path.GetExtension(image.FileName);

                var filePath = Path.Combine(folderPath, fileName);
                using (FileStream ms = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(ms);
                }
                string relativePath = Path.Combine("images", "user", idAndDate, "uploads", roomid.ToString(), fileName);
                picture += relativePath + ";";
            }
            return picture;
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
        public string CheckFileExists(string imagePath)
        {
            string existingImages = "";
            if (imagePath != null)
            {
                string[] filePaths = imagePath.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string image in filePaths)
                {
                    string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image);
                    if (File.Exists(folderPath))
                    {
                        existingImages += image + ";";
                    }
                }
                return existingImages; // file  exist
            }
            else
                return null; // file not exist
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
