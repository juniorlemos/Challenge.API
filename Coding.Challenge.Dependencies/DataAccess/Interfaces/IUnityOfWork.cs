using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coding.Challenge.Dependencies.DataAccess.Interfaces
{
    public interface IUnityOfWork
    {
        public Task Commit();
    }
}
