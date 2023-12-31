﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using motel.Data;
using motel.Models.Domain;
using motel.Models.DTO;
using System.Data;
using System.Globalization;
using System.Net;
using System.Net.WebSockets;
using System.Numerics;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace motel.Repositories
{
    public class UserRepositories : IUserRepositories
    {
        private readonly AppDbContext _dbContext;
        public UserRepositories(AppDbContext context)
        {
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
                datecreated = (DateTime)(addUser.datecreated = DateTime.Now),
                birthday = DateTime.ParseExact(addUser.birthday, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                password = addUser.password,
                phone = addUser.phone,
                roleId = addUser.roleId,
                //tierId = addUser.tierId = 1,
                FileUri = addUser.FileUri,
            };
            _dbContext.User.Add(userDomain);
            _dbContext.SaveChanges();
            if (addUser.FileUri != null)
            {
                addUser.actualFile = UploadImage(addUser.FileUri, userDomain.Id, addUser.datecreated.ToString("yyyy"));
            }
            else
            {
                var defaultimage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "user", "noavt.png");
                var uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "user", userDomain.Id + "-" + userDomain.datecreated.ToString("yyyy"));
                Directory.CreateDirectory(uploadFolderPath);
                var filePath = Path.Combine(uploadFolderPath, "avatar.png");
                File.Copy(defaultimage, filePath, true);
                addUser.actualFile = Path.Combine("images", "user", userDomain.Id + "-" + userDomain.datecreated.ToString("yyyy"), "avatar.png");
            }
            userDomain.actualFile = addUser.actualFile;
            _dbContext.SaveChanges();
            var usertier = new Tier_User()
            {
                tierId = addUser.tierId = 1,
                userId = userDomain.Id,
            };
            _dbContext.Tier_User.Add(usertier);
            _dbContext.SaveChanges();
            return addUser;

        }
        User? IUserRepositories.DeleteUserById(int id)
        {
            var userDomain = _dbContext.User.FirstOrDefault(c => c.Id == id);
            var userPost = _dbContext.Post.Where(up => up.userId == id).ToList();
            var userManager = _dbContext.Post_Manage.Where(um => um.postId == id).ToList();
            var tierDomain = _dbContext.Tier_User.FirstOrDefault(u => u.userId == id);
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
                    _dbContext.Tier_User.Remove(tierDomain);
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

        UserListResult IUserRepositories.GetAllUser(string? filterOn = null, string?
filterQuery = null, int pageNumber = 1, int pageSize = 5)
        {
            var skipResults = (pageNumber - 1) * pageSize;

            var query = _dbContext.User.Include(ut => ut.users_tier).ThenInclude(ut => ut.tiers).Include(r => r.role).Select(user => new UserDTO
            {
                Id = user.Id,
                firstname = user.firstname,
                lastname = user.lastname,
                gender = user.gender ? "Nam" : "Nữ",
                address = user.address,
                password = user.password,
                phone = user.phone,
                tierId = user.users_tier.tierId,
                tier = user.users_tier.tiers.tiername,
                roleId = user.roleId,
                rolename = user.role.rolename,
                birthday = user.birthday.ToString("dd/MM/yyyy"),
                posts = user.post.ToList(),
                datecreated = user.datecreated.ToString("dd/MM/yyyy"),
                actualFile = user.actualFile,
            }).AsSplitQuery();
            //filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("firstname", StringComparison.OrdinalIgnoreCase))
                {
                    var queryat = query.Where(x => x.firstname.Contains(filterQuery) || x.lastname.Contains(filterQuery));
                }
                if (filterOn.Equals("phone", StringComparison.OrdinalIgnoreCase))
                {
                    var queryat = query.Where(x => x.phone.Contains(filterQuery));
                }
                var users = query
                            .OrderBy(u => u.Id)
                            .Skip(skipResults)
                            .Take(pageSize)
                            .ToList();
                var totalUsers = users.Count();
                var totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);
                var result = new UserListResult
                {
                    Users = users,
                    total = totalUsers,
                    TotalPages = totalPages,
                };

                return result;
            }
            else
            {
                var totalUsers = query.Count();
                var totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);
                var users = query
                    .OrderBy(u => u.Id)
                    .Skip(skipResults)
                    .Take(pageSize)
                    .ToList();

                var result = new UserListResult
                {
                    Users = users,
                    total = totalUsers,
                    TotalPages = totalPages,
                };

                return result;
            }
        }
        public UserRoleResult GetAllUserRole(int pageNumber = 1, int pageSize = 10)
        {
            var skipResults = (pageNumber - 1) * pageSize;
            var query = _dbContext.User.Select(u => new UserRoleDTO
            {
                Id = u.Id,
                fullname = u.firstname + " " + u.lastname,
                phone = u.phone,
                rolename = u.role.rolename,
                roleId = u.roleId,
            });
            var totalUsers = query.Count();
            var totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);

            var users = query
                .OrderBy(u => u.Id)
                .Skip(skipResults)
                .Take(pageSize)
                .ToList();
            var result = new UserRoleResult
            {
                Users = users,
                total = totalUsers,
                TotalPages = totalPages,
            };
            return result;
        }
        public EditUserRoleDTO EditUserRole(EditUserRoleDTO editroleDTO)
        {
            foreach (int userIds in editroleDTO.userids)
            {
                var UserDomain = _dbContext.User.FirstOrDefault(n => n.Id == userIds);
                if (UserDomain != null)
                {
                    UserDomain.roleId = editroleDTO.roleid;
                    _dbContext.SaveChanges();
                }
            }
            return editroleDTO;
        }

        UserNoIdDTO IUserRepositories.GetUserById(int id)
        {
            DateTime now = DateTime.Now;
            DateTime oneDayAgo = now.AddDays(-30);
            var postlist = _dbContext.Post.
                Include(pm => pm.post_manage)
                .Include(pm => pm.post_category)
                .ThenInclude(pm => pm.category)
                .Include(pm => pm.user)
                .ThenInclude(pm => pm.users_tier)
                .ThenInclude(pm => pm.tiers).Where(n => n.userId == id)
                .Select(p => new PostDTO()
                {
                    Id = p.Id,
                    title = p.title,
                    address = p.address,
                    authorname = p.user.firstname + " " + p.user.lastname,
                    description = p.description,
                    price = p.price,
                    actualFile = p.actualFile,
                    area = p.area,
                    isHire = p.isHire ? "Đã Được Thuê" : "Chưa Được Thuê",
                    postTier = p.user.users_tier.tiers.tiername,
                    status = p.status,
                    authorid = p.userId,
                    phone = p.user.phone,
                    dateCreated = p.datecreatedroom,
                    expireDate = p.datecreatedroom.AddDays(30).ToString("dd/MM/yyyy"),
                    reason = p.post_manage.reason != null ? p.post_manage.reason.ToString() : "Không Bị Từ Chối Duyệt",
                    FormattedDatecreated = p.datecreatedroom.ToString("dd/MM/yyyy"),
                    FormattedDateapprove = p.post_manage.dateapproved != null ? p.post_manage.dateapproved.Value.ToString("dd/MM/yyyy") : "Chưa Có Ngày Duyệt",
                    categorylist = p.post_category.Select(pc => pc.category.name).ToList(),
                    categoryids = p.post_category.Select(pc => pc.category.Id).ToList(),
                }).AsSplitQuery();
            var UserbyDomain = _dbContext.User.Where(n => n.Id == id);
            var UserWithIdDTO = UserbyDomain.Select(User => new UserNoIdDTO()
            {
                firstname = User.firstname,
                lastname = User.lastname,
                gender = User.gender,
                address = User.address,
                password = User.password,
                phone = User.phone,
                tier = User.users_tier.tiers.tiername,
                rolename = User.role.rolename,
                posts = postlist.ToList(),
                birthday = User.birthday.ToString("dd/MM/yyyy"),
                datecreated = User.datecreated.ToString("dd/MM/yyyy"),
                actualFile = User.actualFile,
            }).FirstOrDefault();
            return UserWithIdDTO;
        }
        UpdateUserBasic? IUserRepositories.UpdateUser(int id, UpdateUserBasic updateUserBasic)
        {
            var userDomain = _dbContext.User.FirstOrDefault(u => u.Id == id);
            if (userDomain != null)
            {
                //    if (updateUserBasic.FileUri != null)
                //    {
                //        updateUserBasic.actualFile = UpdateImage(updateUserBasic.FileUri, userDomain.actualFile, id, userDomain.datecreated.ToString("yyyy"));
                //        userDomain.FileUri = updateUserBasic.FileUri;
                //        userDomain.actualFile = updateUserBasic.actualFile;
                //    }
                //    if (updateUserBasic.FileUri == null && userDomain.actualFile != null)
                //    {
                //        updateUserBasic.actualFile = userDomain.actualFile.ToString();
                //        userDomain.actualFile = updateUserBasic.actualFile;
                //    }
                userDomain.firstname = updateUserBasic.firstname;
                userDomain.lastname = updateUserBasic.lastname;
                userDomain.address = updateUserBasic.lastname;
                userDomain.gender = updateUserBasic.gender;
                userDomain.birthday = DateTime.ParseExact(updateUserBasic.birthday, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                userDomain.phone = updateUserBasic.phone;
                var tierDomain = _dbContext.Tier_User.FirstOrDefault(u => u.userId == id);
                if (updateUserBasic.tierId == 0 && tierDomain != null)
                {
                    updateUserBasic.tierId = tierDomain.tierId;
                }
                else
                {
                    tierDomain.tierId = updateUserBasic.tierId;
                }
                if (updateUserBasic.roleId == 0)
                {
                    updateUserBasic.roleId = userDomain.roleId;
                }
                else
                {
                    userDomain.roleId = updateUserBasic.roleId;
                }
                _dbContext.SaveChanges();

            }

            return updateUserBasic;
        }
        UpdateCustomer? IUserRepositories.updateCustomer(int id, motel.Models.DTO.UpdateCustomer updateCustomer)
        {
            var userDomain = _dbContext.User.FirstOrDefault(u => u.Id == id);
            if (userDomain != null)
            {
                //if (updateCustomer.FileUri != null)
                //{
                //    updateCustomer.actualFile = UpdateImage(updateCustomer.FileUri, userDomain.actualFile, id, userDomain.datecreated.ToString("yyyy"));
                //    userDomain.FileUri = updateCustomer.FileUri;
                //    userDomain.actualFile = updateCustomer.actualFile;
                //}
                //if (updateCustomer.FileUri == null && userDomain.actualFile != null)
                //{
                //    updateCustomer.actualFile = userDomain.actualFile.ToString();
                //    userDomain.actualFile = updateCustomer.actualFile;
                //}
                userDomain.firstname = updateCustomer.firstname;
                userDomain.lastname = updateCustomer.lastname;
                userDomain.address = updateCustomer.address;
                userDomain.gender = updateCustomer.gender;
                userDomain.birthday = DateTime.ParseExact(updateCustomer.birthday, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //var tierDomain = _dbContext.Tier_User.FirstOrDefault(u => u.userId == id);
                //if (updateUserBasic.tierId == 0 && tierDomain != null)
                //{
                //    updateUserBasic.tierId = tierDomain.tierId;
                //}
                //else
                //{
                //    tierDomain.tierId = updateUserBasic.tierId;
                //}
                //if (updateUserBasic.roleId == 0)
                //{
                //    updateUserBasic.roleId = userDomain.roleId;
                //}
                //else
                //{
                //    userDomain.roleId = updateUserBasic.roleId;
                //}
                _dbContext.SaveChanges();

            }

            return updateCustomer;
        }
        AddUserDTO? IUserRepositories.UpdateUserById(int id, AddUserDTO user)
        {
            var userDomain = _dbContext.User.FirstOrDefault(u => u.Id == id);
            if (userDomain != null)
            {
                if (user.FileUri != null)
                {
                    user.actualFile = UpdateImage(user.FileUri, userDomain.actualFile, id, userDomain.datecreated.ToString("yyyy"));
                    userDomain.FileUri = user.FileUri;
                    userDomain.actualFile = user.actualFile;
                }
                if (user.FileUri == null && userDomain.actualFile != null)
                {
                    user.actualFile = userDomain.actualFile.ToString();
                    userDomain.actualFile = user.actualFile;
                }
                userDomain.firstname = user.firstname;
                userDomain.lastname = user.lastname;
                userDomain.gender = user.gender;
                userDomain.address = user.address;
                userDomain.password = user.password;
                userDomain.phone = user.phone;
                //userDomain.tierId = user.tierId;
                userDomain.roleId = user.roleId;
                userDomain.birthday = DateTime.ParseExact(user.birthday, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
                    var newPath = UploadImage(file, id, datecreated);
                    return newPath;
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
                var newPath = UploadImage(file, id, datecreated);
                return newPath;
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
