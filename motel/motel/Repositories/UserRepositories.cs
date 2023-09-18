using Microsoft.EntityFrameworkCore;
using motel.Data;
using motel.Models.Domain;
using motel.Models.DTO;
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
            if (addUser.FileUri != null)
            {
                addUser.birthday = DateTime.ParseExact(addUser.FormattedBirthday, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                addUser.datecreated = DateTime.Now;
                var userDomain = new User
                {
                    firstname = addUser.firstname,
                    lastname = addUser.lastname,
                    address = addUser.address,
                    gender = addUser.gender,
                    datecreated = addUser.datecreated,
                    birthday = addUser.birthday,
                    password = addUser.password,
                    phone = addUser.phone,
                    roleId = addUser.roleId,
                    tierId = addUser.tierId = 1,
                    FileUri = addUser.FileUri,
                };
                addUser.actualFile = UploadImage(addUser.FileUri, userDomain.Id, addUser.datecreated.ToString("yyyy"));
                userDomain.actualFile = addUser.actualFile;
                _dbContext.User.Add(userDomain);
                _dbContext.SaveChanges();
            }
            return addUser;

        }

        User? IUserRepositories.DeleteUserById(int id)
        {
            var userDomain = _dbContext.User.FirstOrDefault(r => r.Id == id);
            if (userDomain != null)
            {
                if (DeleteImage(userDomain.actualFile) == true)
                {
                    DeleteImage(userDomain.actualFile);
                }
                _dbContext.User.Remove(userDomain);
                _dbContext.SaveChanges();
                return userDomain;
            }
            else
            {
                return null;
            }
        }

        List<UserDTO> IUserRepositories.GetAllUser()
        {
            var allUsers = _dbContext.User.Select(User => new UserDTO()
            {
                Id = User.Id,
                fullname = User.firstname +" " +User.lastname,
                gender = User.gender,
                address = User.address,
                password = User.password,
                phone = User.phone,
                tier = User.tiers.tiername,
                rolename = User.role.rolename,
                birthday = User.birthday.ToString("dd/MM/yyyy"),
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
                gender = User.gender,
                address = User.address,
                password = User.password,
                phone = User.phone,
                tier = User.tiers.tiername,
                rolename = User.role.rolename,
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
                user.birthday = DateTime.ParseExact(user.FormattedBirthday, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
                userDomain.birthday = user.birthday;
                userDomain.actualFile = user.actualFile;
                _dbContext.SaveChanges();
            }
            return user;
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
