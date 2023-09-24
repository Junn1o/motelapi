using Microsoft.EntityFrameworkCore;
using motel.Data;
using motel.Models.Domain;
using motel.Models.DTO;
using System.Data;
using System.Globalization;
using System.Net;
using System.Numerics;
using System.Reflection;

namespace motel.Repositories
{
    public class UserRepositories : IUserRepositories
    {
        private readonly AppDbContext _dbContext;
        public UserRepositories(AppDbContext context) {
            _dbContext = context;
        }
        AddUserDTO IUserRepositories.AddUser(AddUserDTO addUser)
        {
            var userDomain = new User
            {
                firstname = addUser.firstname,
                lastname = addUser.lastname,
                address = addUser.address,
                gender = addUser.gender,
                datecreated = addUser.datecreated = DateTime.Now,
                birthday = DateTime.ParseExact(addUser.birthday, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                password = addUser.password,
                phone = addUser.phone,
                roleId = addUser.roleId,
                tierId = addUser.tierId = 1,
                FileUri = addUser.FileUri
            };
            if (addUser.FileUri != null)
            {
                addUser.actualFile = UploadImage(addUser.FileUri, userDomain.Id, addUser.datecreated.ToString("yyyy"));
            }
            else
            {
                var defaultimage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "user", "noavt.png");
                var uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", "images", "user", userDomain.Id + "-" + userDomain.datecreated.ToString("yyyy"));
                Directory.CreateDirectory(uploadFolderPath);
                var filePath = Path.Combine(uploadFolderPath, "avatar.png");
                File.Copy(defaultimage, filePath, true);
                addUser.actualFile = Path.Combine("images", "user", userDomain.Id + "-" + userDomain.datecreated.ToString("yyyy"), "avatar.png");
            }
            userDomain.actualFile = addUser.actualFile;
            _dbContext.User.Add(userDomain);
            _dbContext.SaveChanges();
            return addUser;

        }

        User? IUserRepositories.DeleteUserById(int id)
        {
            var userDomain = _dbContext.User.FirstOrDefault(c => c.Id == id);
            var userPost = _dbContext.Post.Where(up => up.userId == id).ToList();
            var userManager = _dbContext.Post_Manage.Where(um => um.postId == id).ToList();
            if (userDomain != null)
            {
                
                if (userPost.Any())
                {
                    foreach (var post in userPost)
                    {
                        var userPostCategory = _dbContext.Post_Category.Where(n => n.postId == post.Id).ToList();
                        if (userPostCategory.Any())
                        {
                            _dbContext.Post_Category.RemoveRange(userPostCategory);
                            _dbContext.SaveChanges();
                        }
                    }
                    DeleteImage(userDomain.actualFile);
                    _dbContext.Post.RemoveRange(userPost);
                    _dbContext.SaveChanges();
                    _dbContext.User.Remove(userDomain);
                    _dbContext.SaveChanges();
                }
                else
                {
                    _dbContext.User.Remove(userDomain);
                    _dbContext.SaveChanges();
                }
            }
            return userDomain;
        }

        List<UserDTO> IUserRepositories.GetAllUser()
        {
            var allUsers = _dbContext.User.Select(User => new UserDTO()
            {
                Id = User.Id,
                firstname = User.firstname, 
                lastname =User.lastname,
                gender = User.gender ? "Nam" : "Nữ",
                address = User.address,
                password = User.password,
                phone = User.phone,
                tier = User.tiers.tiername,
                rolename = User.role.rolename,
                birthday = User.birthday.ToString("dd/MM/yyyy"),
                posts = User.post.Select(post => post.Id).ToList(),
                datecreated = User.datecreated.ToString("dd/MM/yyyy"),
                actualFile = User.actualFile,
            }).ToList();
            return allUsers;
        }

        UserNoIdDTO IUserRepositories.GetUserById(int id)
        {
            
            var UserbyDomain = _dbContext.User.Where(n => n.Id == id);
            var UserWithIdDTO = UserbyDomain.Select(User => new UserNoIdDTO()
            {
                fullname = User.firstname + " " + User.lastname,
                gender = User.gender ? "Nam" : "Nữ",
                address = User.address,
                password = User.password,
                phone = User.phone,
                tier = User.tiers.tiername,
                rolename = User.role.rolename,
                posts = User.post.Select(post => post.Id).ToList(),
                birthday = User.birthday.ToString("dd/MM/yyyy"),
                datecreated = User.datecreated.ToString("dd/MM/yyyy"),
                actualFile = User.actualFile,
            }).FirstOrDefault();
            return UserWithIdDTO;
        }

        AddUserDTO? IUserRepositories.UpdateUserById(int id,AddUserDTO user)
        {
            var userDomain = _dbContext.User.FirstOrDefault(u => u.Id == id);
            if (userDomain != null)
            {
                if (UpdateImage(user.FileUri, userDomain.actualFile, id, userDomain.datecreated.ToString("yyyy")) == null)
                {
                    user.actualFile = UploadImage(user.FileUri, id, userDomain.datecreated.ToString("yyyy"));
                }
                else
                {
                    user.actualFile = UpdateImage(user.FileUri, userDomain.actualFile, id, userDomain.datecreated.ToString("yyyy"));
                }
                userDomain.firstname = user.firstname;
                userDomain.lastname = user.lastname;
                userDomain.gender = user.gender;
                userDomain.address = user.address;
                userDomain.password = user.password;
                userDomain.phone = user.phone;
                userDomain.tierId = user.tierId;
                userDomain.roleId = user.roleId;
                userDomain.birthday = DateTime.ParseExact(user.birthday, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                userDomain.actualFile = user.actualFile;
                _dbContext.SaveChanges();
            }
            return user;
        }
        User? IUserRepositories.Authenticate(string phone, string password)
        {
            return _dbContext.User.SingleOrDefault(u => u.phone == phone && u.password == password);
        }
        public string UploadImage(IFormFile file, int id, string datecreated)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            var uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "user", id + "-" + datecreated);
            Directory.CreateDirectory(uploadFolderPath);
            var filePath = Path.Combine(uploadFolderPath, "avatar" + fileExtension);
            using (FileStream ms = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(ms);
            }
            var path = Path.Combine("images", "user", id + "-" + datecreated, "avatar" + fileExtension);
            return path;
        }
        public string UpdateImage(IFormFile file, string currentpath, int id, string datecreated)
        {
            if (currentpath != null)
            {
                var oldFullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", currentpath);
                if (!File.Exists(oldFullPath))
                {
                    return null;
                }
                else
                {
                    File.Delete(oldFullPath);
                    var newPath = UploadImage(file, id, datecreated);
                    return newPath;
                }
            }
            else
            {
                return null;
            }
        }
        public bool DeleteImage(string imagePath)
        {
            string parentDirectoryName = Path.GetFileName(Path.GetDirectoryName(imagePath));
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "user", parentDirectoryName);
            if (!Directory.Exists(folderPath))
            {
                return false;
            }
            else
            {
                Directory.Delete(folderPath, true);
                return true;
            }
        }
    }
}
