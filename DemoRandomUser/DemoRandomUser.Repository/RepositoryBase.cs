using DemoRandomUser.Data;

namespace DemoRandomUser.Repository
{
    public class RepositoryBase
    {
        private IDbContext _dbContext;

        public RepositoryBase(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}