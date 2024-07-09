using Coding.Challenge.Dependencies.DataAccess;
using Coding.Challenge.Dependencies.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coding.Challenge.Dependencies.Services
{
    public class UnitOfWork : IUnityOfWork
    {
        private readonly ChallengeDbContext _dbContext;
        
        public UnitOfWork(ChallengeDbContext dbContext) => _dbContext = dbContext;
        public async Task Commit() => await _dbContext.SaveChangesAsync();
        
    }
}
