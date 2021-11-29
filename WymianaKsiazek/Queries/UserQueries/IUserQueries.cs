using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Database.Entities;
using WymianaKsiazek.Models;
using WymianaKsiazek.Models.MapperModels;

namespace WymianaKsiazek.Queries.UserQueries
{
    public interface IUserQueries
    {
        public Task<UserMP> GetUserById(string id); 
        public Task<int> CreateNewUser(CreateUserMP model);
        public (string, DateTime) CreateAccessToken(UserEntity user);
        public Task<string> CreateRefreshToken(string userId);
        public Task<TokenOutput> CreateToken(string email, string password);
        public Task<int> RemoveRefreshToken(string token);
        public MyProfileMP MyProfile(string userid);
        public Task SendEmailResetPassword(UserEntity user);
        public Task ChangePassword(UserEntity user, string password);
        public Task<int> ChangeCurrentPassword(string userid, string oldpasswd, string newpasswd);
        public UserProfile GetUserProfile(string id);
        public List<OffersListMP> GetUsersLikedOffers(string userid);
    }
}
