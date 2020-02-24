#region

using DemoRandomUser.Data;
using RandomUser.Model;
using System.Collections.Generic;
using System.Linq;


#endregion

namespace DemoRandomUser.Repository
{
    public class UserService : ServiceBase<User>
    {
        public UserService(IDbContext db) : base(db)
        {
        }
    }
}