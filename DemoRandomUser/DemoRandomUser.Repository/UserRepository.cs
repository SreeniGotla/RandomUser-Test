using DemoRandomUser.Data;
using DemoRandomUser.Dto;
using RandomUser.Model;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DemoRandomUser.Common.Constants;

namespace DemoRandomUser.Repository
{
    public class UserRepository : RepositoryBase
    {
        private readonly IDbContext _dbContext;
        public UserRepository(IDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultDto<List<User>>> GetUsers()
        {
            var users = _dbContext.Repository<User>()
                .Take(General.Top20User)
                .ToList();

            var result = new ResultDto<List<User>> { Data = users, StatusCode = HttpStatusCode.OK };

            return result;
        }
        public async Task<ResultDto<List<User>>> GetUsersByName(string firstName, string lastName)
        {
            var users = _dbContext.Repository<User>().Where(c => c.UserFirstName.Contains(firstName) || c.UserLastName.Contains(lastName))
                .ToList();

            var result = new ResultDto<List<User>> { Data = users, StatusCode = HttpStatusCode.OK };

            return result;
        }
        public async Task<ResultDto<User>> GetUserById(int userId)
        {
            var user = _dbContext.Repository<User>().Where(x => x.UserId == userId).SingleOrDefault();

            var result = new ResultDto<User> { Data = user, StatusCode = HttpStatusCode.OK };

            return result;
           
        }
        public async Task<ResultDto<User>> Update(User user)
        {
            ResultDto<User> result;
            var userExist = _dbContext.Repository<User>().Where(x => x.UserId == user.UserId).SingleOrDefault();
            if(userExist == null)
            {
                result = new ResultDto<User>
                {
                    Data = user,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessage = "User doesn't exist"
                };
                return result;
            }
            _dbContext.Update(user);
            _dbContext.Commit();

            result = new ResultDto<User>
            {
                Data = user,
                StatusCode = HttpStatusCode.OK
            };

            return result;
        }
        public void Delete(User user)
        {
            _dbContext.Delete(user);
            _dbContext.Commit();
        }
    }
}
