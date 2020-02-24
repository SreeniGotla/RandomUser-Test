using DemoRandomUser.Dto;
using DemoRandomUser.Repository;
using RandomUser.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace DemoRandomUser.Controllers
{
    [Route("User")]
    public class UserController : ApiController
    {
        private readonly UserRepository _userRepository;
        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Route("User")]
        public Task<ResultDto<List<User>>> Get()
        {
            var result = _userRepository.GetUsers();

            return result;
        }

        [Route("User/{userId}")]
        public Task<ResultDto<User>> Get(int userId)
        {
            var result = _userRepository.GetUserById(userId);

            return result;
        }

        [Route("User/GetUsersByName")]
        [HttpPost]
        public Task<ResultDto<List<User>>> GetUsersByName(string firstName, string lastName)
        {
            var result = _userRepository.GetUsersByName(firstName, lastName);

            return result;
        }

        [HttpPut]
        [Route("User/Update")]
        public async Task<ResultDto<User>> Update(User usr)
        {
            var result = _userRepository.Update(usr);

            return await result;
        }

        [HttpDelete]
        [Route("User/{userId}")]
        public async Task<IHttpActionResult> Delete(int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user.Data == null) return NotFound();

            _userRepository.Delete(user.Data);
            return Ok();
        }
    }
}